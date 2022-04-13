using Autofac;
using FPN.Core.Services;

namespace FPN.Core
{
	public class CoreModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<DialogService>().As<IDialogService>();
		}
	}
}