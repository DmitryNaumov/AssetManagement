namespace AssetManagement.Core.Events
{
	using System;
	using AssetManagement.Infrastructure;

	internal sealed class AssetsMerged : CorrelatedBy
	{
		public AssetsMerged(Guid correlationId, Guid hostId)
		{
			CorrelationId = correlationId;
			HostId = hostId;
		}

		public Guid CorrelationId { get; private set; }

		public Guid HostId { get; private set; }
	}
}