﻿namespace AssetManagement.Core.Commands
{
	using System;
	using AssetManagement.Contracts.Identities;
	using AssetManagement.Infrastructure;
	using AssetManagement.Infrastructure.Messaging;

	internal sealed class ResolveHost : Message
	{
		public ResolveHost(HostIdentity[] hostIdentities)
		{
			HostIdentities = hostIdentities;
		}

		public HostIdentity[] HostIdentities { get; private set; }
	}
}
