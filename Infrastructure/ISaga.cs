namespace AssetManagement.Infrastructure
{
	using System;
	using AssetManagement.Infrastructure.Persistence;

	public interface ISaga
	{
		Guid Id { get; }
	}

	public interface ISaga<T> : ISaga, IOptimisticConcurrencyAware, IConsumer<T> where T : CorrelatedBy
	{
	}

	public interface CorrelatedBy
	{
		Guid CorrelationId { get; }
	}
}