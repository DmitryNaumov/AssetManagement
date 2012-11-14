namespace AssetManagement.Infrastructure.Persistence
{
	using System;

	[Serializable]
	public sealed class OptimisticConcurrencyException : Exception
	{
	}
}