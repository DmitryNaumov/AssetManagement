namespace AssetManagement.Contracts.Assets
{
	using System;

	[Serializable]
	public sealed class OperatingSystem : SoftwareAsset
	{
		public OperatingSystem(string name)
		{
			Name = name;
		}

		public string Name { get; private set; }
	}
}
