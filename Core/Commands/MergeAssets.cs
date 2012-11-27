namespace AssetManagement.Core.Commands
{
	using System;
	using AssetManagement.Contracts.Assets;
	using AssetManagement.Infrastructure;

	internal sealed class MergeAssets : Message
	{
		public MergeAssets(Guid hostId, int version, Asset[] assets)
		{
			HostId = hostId;
			Version = version;
			Assets = assets;
		}

		public Guid HostId { get; private set; }

		public int Version { get; private set; }

		public Asset[] Assets { get; private set; }
	}
}
