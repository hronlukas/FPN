using FPN.Bussines;
using FPN.Bussines.Services;
using FPN.Core;
using FPN.Core.Mvvm;
using FPN.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;
using System;
using System.Linq;
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

		private readonly IHost host;

		static App()
		{
			loggerFactory = new LoggerFactory([new DebugLoggerProvider(),]);
			logger = loggerFactory.CreateLogger<App>();
		}

		public App()
		{
			AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
			Current.DispatcherUnhandledException += DispatcherOnUnhandledException;
			TaskScheduler.UnobservedTaskException += TaskSchedulerOnUnobservedTaskException;

			host = Host.CreateDefaultBuilder()
				.ConfigureServices((context, services) =>
				{
					ConfigureServices(services);
				})
				.Build();

			// Set service provider to static resolver to resolve correct view model as a data context in view
			DependencyInjectionWpf.Resolver = host.Services.GetRequiredService;
		}

		protected override async void OnExit(ExitEventArgs e)
		{
			IUserEditor editor = host.Services.GetRequiredService<IUserEditor>();
			editor.Save();

			await host.StopAsync();
			host.Dispose();
			base.OnExit(e);
		}

		protected override async void OnStartup(StartupEventArgs e)
		{
			logger.LogDebug("Dispatcher managed thread identifier = {threadId}", Environment.CurrentManagedThreadId);

			const string RenderCapabilityMessage = "WPF rendering capability (tier) = {renderCapability}";
			logger.LogDebug(RenderCapabilityMessage, RenderCapability.Tier / 0x10000);
			RenderCapability.TierChanged += (s, a) => logger.LogDebug(RenderCapabilityMessage, RenderCapability.Tier / 0x10000);

			await host.StartAsync();

			base.OnStartup(e);
		}

		private static void AddViewModels(IServiceCollection services)
		{
			var viewModelTypes = System.Reflection.Assembly.GetExecutingAssembly()
				.GetTypes()
				.Where(t => typeof(ViewModelBase).IsAssignableFrom(t));

			foreach (var type in viewModelTypes)
			{
				services.AddTransient(type);
			}
		}

		private static void ConfigureServices(IServiceCollection services)
		{
			// Services
			// todo

			// View models
			AddViewModels(services);

			// Modules
			services.AddCore();
			services.AddBussines();

			// Main window
			services.AddSingleton<MainWindow>();
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
	}
}