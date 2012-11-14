namespace AssetManagement.Contracts.Assets
{
	using System;

	[Serializable]
	public sealed class WebBrowser : SoftwareAsset
	{
		public WebBrowser(string name)
		{
			Name = name;
		}

		public string Name { get; private set; }
	}
}
