namespace AssetManagement.Core.Handlers
{
	using System;
	using System.Collections.Generic;
	using AssetManagement.Contracts.Assets;
	using AssetManagement.Core.Commands;
	using AssetManagement.Core.Events;
	using AssetManagement.Infrastructure;
	using AssetManagement.Infrastructure.Persistence;

	internal sealed class MergeAssetsHandler : IConsumer<MergeAssets>
	{
		private readonly IServiceBus _serviceBus;
		private readonly IRepository _repository;

		public MergeAssetsHandler(IServiceBus serviceBus, IRepository repository)
		{
			_serviceBus = serviceBus;
			_repository = repository;
		}

		public void Handle(MergeAssets message)
		{
			var host = GetOrCreateHost(message.HostId);
			if (host.Version != message.Version)
			{
				// something has changed from the time we resolved host by identities, let saga re-run resolve process
				_serviceBus.Publish(new MergeRejected(message.CorrelationId, host.Id));

				return;
			}

			host.MergeAssets(message.Assets);

			_repository.Save(host.Id, host);

			_serviceBus.Publish(new AssetsMerged(message.CorrelationId, host.Id));
		}

		private Host GetOrCreateHost(Guid hostId)
		{
			return (Host)_repository.Get(hostId) ?? new Host(hostId);
		}

		[Serializable]
		private class Host : IOptimisticConcurrencyAware
		{
			private readonly Guid _id;

			public Host(Guid id)
			{
				_id = id;
			}

			public Guid Id
			{
				get { return _id; }
			}

			public int Version { get; set; }

			public void MergeAssets(IEnumerable<Asset> assets)
			{
				// TODO:
			}
		}
	}
}
