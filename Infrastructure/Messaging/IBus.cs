namespace AssetManagement.Infrastructure.Messaging
{
	using System;

	public interface IBus
	{
		void Publish(object message, Action<ISendContext> contextCallback = null);

		bool IsIdle();
	}
}
