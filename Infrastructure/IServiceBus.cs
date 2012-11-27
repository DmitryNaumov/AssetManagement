namespace AssetManagement.Infrastructure
{
	using System;

	public interface IServiceBus
	{
		void Publish(object message, Action<ISendContext> contextCallback = null);

		bool IsIdle();
	}
}
