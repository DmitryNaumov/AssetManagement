namespace AssetManagement.Contracts.Assets
{
	using System;

	[Serializable]
	public abstract class Asset
	{
		//public abstract AssetClass AssetClass { get; }
	}

	public enum AssetClass
	{
		Unknown = 0,

		OperatingSystem = 100000,
		WebBrowser = 100001,
		WebServer = 100002,
	}
}
