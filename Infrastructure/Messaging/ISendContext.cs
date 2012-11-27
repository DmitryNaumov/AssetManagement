namespace AssetManagement.Infrastructure.Messaging
{
	using System;

	public interface ISendContext
	{
		void SetCorrelationId(Guid? correlationId);
	}
}