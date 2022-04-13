using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.CommonServiceLocator;
using CommonServiceLocator;
using FPN.Bussines;
using FPN.Bussines.Services;
using FPN.Core;
using FPN.Core.Mvvm;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace FPN
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		private static readonly ILogger logger;
		private static readonly ILoggerFactory loggerFactory;
		private IContainer? container;

		static App()
		{
			loggerFactory = new LoggerFactory(new ILoggerProvider[] { new DebugLoggerProvider(), });
			logger = loggerFactory.CreateLogger<App>();
		}

		public App()
		{
			AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
			Current.DispatcherUnhandledException += DispatcherOnUnhandledException;
			TaskScheduler.UnobservedTaskException += TaskSchedulerOnUnobservedTaskException;
		}

		protected override void OnStartup(StartupEventArgs e)
		{
			logger.LogDebug("Dispatcher managed thread identifier = {threadId}", Environment.CurrentManagedThreadId);

			const string RenderCapabilityMessage = "WPF rendering capability (tier) = {renderCapability}";
			logger.LogDebug(RenderCapabilityMessage, RenderCapability.Tier / 0x10000);
			RenderCapability.TierChanged += (s, a) => logger.LogDebug(RenderCapabilityMessage, RenderCapability.Tier / 0x10000);

			base.OnStartup(e);

			ConfigureServices();
		}

		protected override void OnExit(ExitEventArgs e)
		{
			var editor = container?.Resolve<IUserEditor>();
			editor?.Save();
			base.OnExit(e);
		}

		private static void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs args)
		{
			logger.LogInformation("Unhandled app domain exception");
			HandleException(args.ExceptionObject as Exception);
		}

		private static void DispatcherOnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs args)
		{
			logger.LogInformation("Unhandled dispatcher thread exception");
			args.Handled = true;

			HandleException(args.Exception);
		}

		private static void HandleException(Exception? exception)
		{
			logger.LogError(exception, "Unhandled application exception");
		}

		private static void TaskSchedulerOnUnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
		{
			logger.LogInformation("Unhandled task exception");
			e.SetObserved();

			HandleException(e.Exception.GetBaseException());
		}

		private void ConfigureServices()
		{
			// The Microsoft.Extensions.DependencyInjection.ServiceCollection
			// has extension methods provided by other .NET Core libraries to
			// register services with DI.
			var serviceCollection = new ServiceCollection();

			// The Microsoft.Extensions.Logging package provides this one-liner
			// to add logging services.
			serviceCollection.AddLogging();

			var containerBuilder = new ContainerBuilder();

			// Once you've registered everything in the ServiceCollection, call
			// Populate to bring those registrations into Autofac. This is
			// just like a foreach over the list of things in the collection
			// to add them to Autofac.
			containerBuilder.Populate(serviceCollection);

			// Make your Autofac registrations. Order is important!
			// If you make them BEFORE you call Populate, then the
			// registrations in the ServiceCollection will override Autofac
			// registrations; if you make them AFTER Populate, the Autofac
			// registrations will override. You can make registrations
			// before or after Populate, however you choose.

			// Logger
			containerBuilder.RegisterInstance(loggerFactory).As<ILoggerFactory>();
			containerBuilder.RegisterGeneric(typeof(Logger<>)).As(typeof(ILogger<>));

			// Services
			// todo

			// View models
			containerBuilder.RegisterAssemblyTypes(System.Reflection.Assembly.GetExecutingAssembly())
				.Where(t => typeof(ViewModelBase).IsAssignableFrom(t));

			// Modules
			containerBuilder.RegisterModule<CoreModule>();
			containerBuilder.RegisterModule<BussinesModule>();

			// Build container
			container = containerBuilder.Build();

			var serviceLocator = new AutofacServiceLocator(container);
			ServiceLocator.SetLocatorProvider(() => serviceLocator);
		}
	}
}