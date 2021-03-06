﻿namespace AssetManagement.Core.Handlers
{
	using System;
	using System.Collections.Generic;
	using AssetManagement.Contracts.Assets;
	using AssetManagement.Core.Commands;
	using AssetManagement.Core.Events;
	using AssetManagement.Infrastructure;
	using AssetManagement.Infrastructure.Messaging;
	using AssetManagement.Infrastructure.Persistence;

	internal sealed class MergeAssetsHandler : IConsumer<MergeAssets>
	{
		private readonly IRepository _repository;

		public MergeAssetsHandler(IRepository repository)
		{
			_repository = repository;
		}

		public void Handle(MergeAssets message)
		{
			var host = GetOrCreateHost(message.HostId);
			if (host.Version != message.Version)
			{
				// something has changed from the time we resolved host by identities, let saga re-run resolve process
				message.ReplyWith(new MergeRejected(host.Id));

				return;
			}

			host.MergeAssets(message.Assets);

			_repository.Save(host.Id, host);

			message.ReplyWith(new AssetsMerged(host.Id));
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
