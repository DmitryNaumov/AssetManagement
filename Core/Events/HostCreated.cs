namespace AssetManagement.Core.Events
{
	using System;
	using AssetManagement.Contracts.Identities;
	using AssetManagement.Infrastructure;
	using AssetManagement.Infrastructure.Messaging;

	internal sealed class HostCreated : Message
	{
		public HostCreated(Guid hostId, int version, HostIdentity[] hostIdentities)
		{
			HostId = hostId;
			Version = version;
			HostIdentities = hostIdentities;
		}

		public Guid HostId { get; private set; }

		public int Version { get; private set; }

		public HostIdentity[] HostIdentities { get; private set; }
	}
}