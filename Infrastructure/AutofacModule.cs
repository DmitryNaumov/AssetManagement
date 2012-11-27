namespace AssetManagement.Infrastructure
{
	using AssetManagement.Infrastructure.Messaging;
	using AssetManagement.Infrastructure.Persistence;
	using Autofac;

	internal sealed class AutofacModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			base.Load(builder);

			builder.RegisterType<InMemoryBus>().AsImplementedInterfaces().SingleInstance();
			builder.RegisterType<Repository>().AsImplementedInterfaces().SingleInstance();
		}
	}
}
