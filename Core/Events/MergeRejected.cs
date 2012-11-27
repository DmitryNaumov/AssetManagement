namespace AssetManagement.Core.Events
{
	using System;
	using AssetManagement.Infrastructure;
	using AssetManagement.Infrastructure.Messaging;

	internal sealed class MergeRejected : Message
	{
		public MergeRejected(Guid hostId)
		{
			HostId = hostId;
		}

		public Guid HostId { get; private set; }
	}
}
