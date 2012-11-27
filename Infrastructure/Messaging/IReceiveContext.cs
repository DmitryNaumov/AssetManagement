namespace AssetManagement.Infrastructure.Messaging
{
	using System;

	public interface IReceiveContext
	{
		Guid? CorrelationId { get; }
	}
}