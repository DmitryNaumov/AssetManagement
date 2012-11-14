namespace AssetManagement.Core.Events
{
	using System;
	using AssetManagement.Contracts.Identities;
	using AssetManagement.Infrastructure;

	internal sealed class HostCreated : CorrelatedBy
	{
		public HostCreated(Guid correlationId, Guid hostId, int version, HostIdentity[] hostIdentities)
		{
			CorrelationId = correlationId;
			HostId = hostId;
			Version = version;
			HostIdentities = hostIdentities;
		}

		public Guid CorrelationId { get; private set; }

		public Guid HostId { get; private set; }

		public int Version { get; private set; }

		public HostIdentity[] HostIdentities { get; private set; }
	}
}