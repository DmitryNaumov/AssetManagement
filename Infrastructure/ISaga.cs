namespace AssetManagement.Infrastructure
{
	using System;
	using AssetManagement.Infrastructure.Persistence;

	public interface ISaga
	{
		Guid Id { get; }
	}

	public interface ISaga<T> : ISaga, IOptimisticConcurrencyAware, IConsumer<T>
	{
	}

	public static class SagaExtensions
	{
		public static void Publish(this ISaga saga, object message)
		{
			ReceiveContext.Current.ServiceBus.Publish(message, x => x.SetCorrelationId(saga.Id));
		}
	}
}