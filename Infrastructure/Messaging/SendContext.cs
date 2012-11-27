namespace AssetManagement.Infrastructure.Messaging
{
	using System;

	internal sealed class SendContext : ISendContext
	{
		private readonly Envelope _envelope;

		public SendContext(Envelope envelope)
		{
			_envelope = envelope;
		}

		public void SetCorrelationId(Guid? correlationId)
		{
			_envelope.CorrelationId = correlationId;
		}
	}
}