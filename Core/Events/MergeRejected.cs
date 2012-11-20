namespace AssetManagement.Core.Events
{
	using System;
	using AssetManagement.Infrastructure;

	internal sealed class MergeRejected : CorrelatedBy
	{
		public MergeRejected(Guid correlationId, Guid hostId)
		{
			CorrelationId = correlationId;
			HostId = hostId;
		}

		public Guid CorrelationId { get; private set; }

		public Guid HostId { get; private set; }
	}
}
