namespace AssetManagement.Core.Events
{
	using System;
	using AssetManagement.Contracts.Assets;
	using AssetManagement.Contracts.Identities;
	using AssetManagement.Infrastructure;

	internal sealed class AssetsFound : CorrelatedBy
	{
		public AssetsFound(HostIdentity[] hostIdentities, Asset[] assets)
		{
			CorrelationId = Guid.NewGuid();
			HostIdentities = hostIdentities;
			Assets = assets;
		}

		public Guid CorrelationId { get; private set; }

		public HostIdentity[] HostIdentities { get; private set; }

		public Asset[] Assets { get; private set; }

		public static AssetsFound New(HostIdentity hostIdentity, params Asset[] assets)
		{
			return new AssetsFound(new [] { hostIdentity }, assets);
		}
	}
}
