namespace AssetManagement.Infrastructure.Persistence
{
	public interface IOptimisticConcurrencyAware
	{
		int Version { get; set; }
	}
}