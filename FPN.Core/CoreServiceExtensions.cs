using FPN.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FPN.Core
{
	public static class CoreServiceExtensions
	{
		public static void AddCore(this IServiceCollection services)
		{
			services.AddScoped<IDialogService, DialogService>();
		}
	}
}