using Microsoft.Extensions.DependencyInjection;

namespace FPN.Core
{
	public class ServiceLocator
	{
		private readonly ServiceProvider _currentServiceProvider;
		private static ServiceProvider? _serviceProvider;

		public ServiceLocator(ServiceProvider currentServiceProvider)
		{
			_currentServiceProvider = currentServiceProvider ?? throw new ArgumentNullException(nameof(currentServiceProvider));
		}

		public static ServiceLocator Current => new(_serviceProvider!);

		public static void SetLocatorProvider(ServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider;
		}

		public object? GetInstance(Type serviceType)
		{
			return _currentServiceProvider.GetService(serviceType);
		}

		public TService? GetInstance<TService>()
		{
			return _currentServiceProvider.GetService<TService>();
		}

		public TService GetRequiredService<TService>() where TService : notnull
		{
			return _currentServiceProvider.GetRequiredService<TService>();
		}
	}
}