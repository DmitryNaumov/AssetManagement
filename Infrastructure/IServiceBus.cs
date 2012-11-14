namespace AssetManagement.Infrastructure
{
	public interface IServiceBus
	{
		void Publish(object message);
		bool IsIdle();
	}
}
