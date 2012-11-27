namespace AssetManagement.Core.Events
{
	using System;
	using AssetManagement.Infrastructure;
	using AssetManagement.Infrastructure.Messaging;

	internal sealed class AssetsMerged : Message
	{
		public AssetsMerged(Guid hostId)
		{
			HostId = hostId;
		}

		public Guid HostId { get; private set; }
	}
}