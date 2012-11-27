namespace AssetManagement.Tests
{
	using AssetManagement.Contracts.Assets;
	using AssetManagement.Contracts.Identities;
	using AssetManagement.Core.Events;
	using AssetManagement.Infrastructure;
	using AssetManagement.Infrastructure.Messaging;
	using Autofac;
	using Microsoft.VisualStudio.TestTools.UnitTesting;

	[TestClass]
	public class SimpleTestScenario : Scenario, IConsumer<AssetsMerged>
	{
		private bool _messageReceived;

		[TestMethod]
		public void SmokeTest()
		{
		}

		[When]
		private void WhenAssetsFound(IBus bus)
		{
			var @event = AssetsFound.New(new MediaAccessControlAddress("A"), new OperatingSystem("Windows"), new WebBrowser("Chrome"));
			bus.Replay(@event);
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

		void IConsumer<AssetsMerged>.Handle(AssetsMerged message)
		{
			_messageReceived = true;
		}
	}
}
