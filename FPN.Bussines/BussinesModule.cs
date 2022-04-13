using Autofac;
using FPN.Bussines.Services;

namespace FPN.Bussines
{
	public class BussinesModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<ImportService>().As<IImportService>();
			builder.RegisterType<XmlInvoiceConverter>().As<IDataConverter>();
			builder.RegisterType<CalculatorProvider>().As<ICalculatorProvider>();

			builder.RegisterType<UserStorageJson>().As<IUserStorage>();
			builder.RegisterType<UserProvider>().As<IUserProvider>().As<IUserEditor>();
		}
	}
}