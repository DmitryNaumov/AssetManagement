namespace AssetManagement.Core.Commands
{
	using System;
	using AssetManagement.Contracts.Identities;
	using AssetManagement.Infrastructure;

	internal sealed class ResolveHost : CorrelatedBy
	{
		public ResolveHost(Guid correlationId, HostIdentity[] hostIdentities)
		{
			CorrelationId = correlationId;
			HostIdentities = hostIdentities;
		}

		public Guid CorrelationId { get; private set; }

		public HostIdentity[] HostIdentities { get; private set; }
	}
}
