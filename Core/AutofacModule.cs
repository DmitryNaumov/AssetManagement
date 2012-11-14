namespace AssetManagement.Core
{
	using AssetManagement.Core.Handlers;
	using AssetManagement.Core.Sagas;
	using Autofac;

	internal sealed class AutofacModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			base.Load(builder);

			builder.RegisterType<Saga>().AsImplementedInterfaces();
			builder.RegisterType<HostListController>().AsImplementedInterfaces();
		}
	}
}
