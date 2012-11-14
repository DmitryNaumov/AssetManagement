﻿namespace AssetManagement.Tests
{
	using AssetManagement.Contracts.Assets;
	using AssetManagement.Contracts.Identities;
	using AssetManagement.Core.Events;
	using AssetManagement.Infrastructure;
	using Autofac;
	using Microsoft.VisualStudio.TestTools.UnitTesting;

	[TestClass]
	public class SimpleTestScenario : Scenario, IConsumer<HostCreated>
	{
		private bool _messageReceived;

		[TestMethod]
		public void SmokeTest()
		{
		}

		[When]
		private void WhenAssetsFound(IServiceBus serviceBus)
		{
			var @event = AssetsFound.New(new MediaAccessControlAddress("A"), new OperatingSystem("Windows"), new WebBrowser("Chrome"));
			serviceBus.Replay(@event);
		}

		[Then]
		private void ThenMessagePublished()
		{
			Assert.IsTrue(_messageReceived);
		}

		protected override void OverrideRegistrations(ContainerBuilder builder)
		{
			builder.RegisterInstance(this);
		}

		void IConsumer<HostCreated>.Handle(HostCreated message)
		{
			_messageReceived = true;
		}
	}
}
