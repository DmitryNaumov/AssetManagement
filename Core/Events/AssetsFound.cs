namespace AssetManagement.Core.Events
{
	using System;
	using AssetManagement.Contracts.Assets;
	using AssetManagement.Contracts.Identities;
	using AssetManagement.Infrastructure;
	using AssetManagement.Infrastructure.Messaging;

	internal sealed class AssetsFound : Message
	{
		public AssetsFound(HostIdentity[] hostIdentities, Asset[] assets)
		{
			HostIdentities = hostIdentities;
			Assets = assets;
		}

		public HostIdentity[] HostIdentities { get; private set; }

		public Asset[] Assets { get; private set; }

		public static AssetsFound New(HostIdentity hostIdentity, params Asset[] assets)
		{
			return new AssetsFound(new [] { hostIdentity }, assets);
		}
	}
}
