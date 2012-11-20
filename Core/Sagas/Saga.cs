namespace AssetManagement.Core.Sagas
{
	using System;
	using AssetManagement.Contracts.Assets;
	using AssetManagement.Contracts.Identities;
	using AssetManagement.Core.Commands;
	using AssetManagement.Core.Events;
	using AssetManagement.Infrastructure;

	[Serializable]
	internal sealed class Saga : ISaga<AssetsFound>, IConsumer<HostResolved>, IConsumer<HostCreated>, IConsumer<HostResolutionAmbiguityDetected>, IConsumer<MergeRejected>, IConsumer<AssetsMerged>
	{
		[NonSerialized] // TODO: should be able to reply to message without declaring dependendcy to IServiceBus
		private readonly IServiceBus _serviceBus;
		private readonly Guid _id;

		private Asset[] _assets;
		private HostIdentity[] _hostIdentities;

		public Saga(IServiceBus serviceBus, Guid id)
		{
			_serviceBus = serviceBus;
			_id = id;
		}

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

			_serviceBus.Publish(new ResolveHost(_id, _hostIdentities));
		}

		public void Handle(HostResolved message)
		{
			_serviceBus.Publish(new MergeAssets(Id, message.HostId, message.Version, _assets));
		}

		public void Handle(HostCreated message)
		{
			_serviceBus.Publish(new MergeAssets(Id, message.HostId, message.Version, _assets));
		}

		public void Handle(HostResolutionAmbiguityDetected message)
		{
			// TODO: send alert email and complete
		}

		public void Handle(MergeRejected message)
		{
			// host identity has changed, re-run resolve process
			_serviceBus.Publish(new ResolveHost(_id, _hostIdentities));
		}

		public void Handle(AssetsMerged message)
		{
			// TODO:
			// Complete();
		}
	}
}
