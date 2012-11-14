namespace AssetManagement.Core.Sagas
{
	using System;
	using AssetManagement.Contracts.Assets;
	using AssetManagement.Core.Commands;
	using AssetManagement.Core.Events;
	using AssetManagement.Infrastructure;

	[Serializable]
	internal sealed class Saga : ISaga<AssetsFound>, IConsumer<HostResolved>, IConsumer<HostCreated>, IConsumer<HostResolutionAmbiguityDetected>
	{
		[NonSerialized] // TODO: should be able to reply to message without declaring dependendcy to IServiceBus
		private readonly IServiceBus _serviceBus;
		private readonly Guid _id;

		private Asset[] _assets;

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

			_serviceBus.Publish(new ResolveHost(_id, @event.HostIdentities));
		}

		public void Handle(HostResolved message)
		{
		}

		public void Handle(HostCreated message)
		{
		}

		public void Handle(HostResolutionAmbiguityDetected message)
		{
		}
	}
}
