namespace AssetManagement.Tests
{
	using System;

	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
	public sealed class GivenAttribute : Attribute
	{
	}
}