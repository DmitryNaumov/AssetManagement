namespace AssetManagement.Infrastructure
{
	public interface IConsumer
	{
	}

	public interface IConsumer<T> : IConsumer
	{
		void Handle(T message);
	}
}