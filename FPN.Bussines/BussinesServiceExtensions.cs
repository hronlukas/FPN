using FPN.Bussines.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FPN.Bussines
{
	public static class BussinesServiceExtensions
	{
		public static void AddBussines(this IServiceCollection services)
		{
			services.AddScoped<IImportService, ImportService>();
			services.AddScoped<IDataConverter, XmlInvoiceConverter>();
			services.AddScoped<ICalculatorProvider, CalculatorProvider>();

			services.AddScoped<IUserStorage, UserStorageJson>();
			services.AddScoped<UserProvider>();
			services.AddScoped<IUserProvider>(x => x.GetRequiredService<UserProvider>());
			services.AddScoped<IUserEditor>(x => x.GetRequiredService<UserProvider>());
		}
	}
}