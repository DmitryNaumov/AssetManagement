namespace AssetManagement.Infrastructure
{
	using AssetManagement.Infrastructure.Persistence;
	using Autofac;

	internal sealed class AutofacModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			base.Load(builder);

			builder.RegisterType<ServiceBus>().AsImplementedInterfaces().SingleInstance();
			builder.RegisterType<Repository>().AsImplementedInterfaces().SingleInstance();
		}
	}
}
