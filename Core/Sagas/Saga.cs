namespace AssetManagement.Core.Sagas
{
	using System;
	using AssetManagement.Contracts.Assets;
	using AssetManagement.Contracts.Identities;
	using AssetManagement.Core.Commands;
	using AssetManagement.Core.Events;
	using AssetManagement.Infrastructure;
	using AssetManagement.Infrastructure.Messaging;

	[Serializable]
	internal sealed class Saga : ISaga<AssetsFound>, IConsumer<HostResolved>, IConsumer<HostCreated>, IConsumer<HostResolutionAmbiguityDetected>, IConsumer<MergeRejected>, IConsumer<AssetsMerged>
	{
		private readonly Guid _id = Guid.NewGuid();

		private Asset[] _assets;
		private HostIdentity[] _hostIdentities;

		public Guid Id
		{
			get { return _id; }
		}

		public int Version { get; set; }

		public void Handle(AssetsFound @event)
		{
			// save for later
			_assets = @event.Assets;
			_hostIdentities = @event.HostIdentities;

			this.Publish(new ResolveHost(_hostIdentities));
		}

		public void Handle(HostResolved message)
		{
			this.Publish(new MergeAssets(message.HostId, message.Version, _assets));
		}

		public void Handle(HostCreated message)
		{
			this.Publish(new MergeAssets(message.HostId, message.Version, _assets));
		}

		public void Handle(HostResolutionAmbiguityDetected message)
		{
			// TODO: send alert email and complete
		}

		public void Handle(MergeRejected message)
		{
			// host identity has changed, re-run resolve process
			this.Publish(new ResolveHost(_hostIdentities));
		}

		public void Handle(AssetsMerged message)
		{
			// TODO:
			// Complete();
		}
	}
}
