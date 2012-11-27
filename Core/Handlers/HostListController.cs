namespace AssetManagement.Core.Handlers
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using AssetManagement.Contracts.Identities;
	using AssetManagement.Core.Commands;
	using AssetManagement.Core.Events;
	using AssetManagement.Infrastructure;
	using AssetManagement.Infrastructure.Persistence;

	internal sealed class HostListController : IConsumer<ResolveHost>
	{
		private static readonly Guid Key = Guid.NewGuid();

		private readonly IRepository _repository;

		public HostListController(IRepository repository)
		{
			_repository = repository;
		}

		public void Handle(ResolveHost message)
		{
			// TODO: should be part of infrastructure?...
			while (true)
			{
				try
				{
					var state = (InternalState)_repository.Get(Key) ?? new InternalState();
					var response = ResolveHost(state, message);
					_repository.Save(Key, state);

					message.ReplyWith(response);
					break;
				}
				catch (OptimisticConcurrencyException ex)
				{
					// that's ok
				}
			}
		}

		private object ResolveHost(InternalState state, ResolveHost message)
		{
			// TODO: host resolving logic must be more complicated, possibly customizable and rule-based

			foreach (var host in state.Hosts)
			{
				if (message.HostIdentities.All(identity => host.Value.Contains(identity)))
				{
					return new HostResolved(host.Key.Item1, host.Key.Item2, message.HostIdentities);
				}

				if (message.HostIdentities.Any(identity => host.Value.Contains(identity)))
				{
					return new HostResolutionAmbiguityDetected();
				}
			}

			// no host found, let's create a new one
			var hostId = Guid.NewGuid();
			state.Hosts.Add(Tuple.Create(hostId, 0), new List<HostIdentity>(message.HostIdentities));

			return new HostCreated(hostId, 0, message.HostIdentities);
		}

		[Serializable]
		private class InternalState : IOptimisticConcurrencyAware
		{
			private readonly Dictionary<Tuple<Guid, int>, List<HostIdentity>> _hosts = new Dictionary<Tuple<Guid, int>, List<HostIdentity>>();

			public Dictionary<Tuple<Guid, int>, List<HostIdentity>> Hosts
			{
				get { return _hosts; }
			}

			public int Version { get; set; }
		}
	}
}
