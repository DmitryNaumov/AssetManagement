namespace AssetManagement.Contracts.Identities
{
	using System;

	[Serializable]
	public sealed class MediaAccessControlAddress : HostIdentity
	{
		public MediaAccessControlAddress(string address)
		{
			Address = address;
		}

		public string Address { get; private set; }

		public override bool Equals(object obj)
		{
			return Equals(obj as MediaAccessControlAddress);
		}

		public bool Equals(MediaAccessControlAddress obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			
			return string.Equals(Address, obj.Address);
		}

		public override int GetHashCode()
		{
			return Address.GetHashCode();
		}
	}
}
