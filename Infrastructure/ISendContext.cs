namespace AssetManagement.Infrastructure
{
	using System;

	public interface ISendContext
	{
		void SetCorrelationId(Guid? correlationId);
	}

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

	internal abstract class Envelope
	{
		public Guid? CorrelationId { get; set; }

		public abstract Type MessageType { get; }
	}

	internal sealed class Envelope<TMessage> : Envelope where TMessage : Message
	{
		private readonly TMessage _message;

		public Envelope(TMessage message)
		{
			_message = message;
		}

		public TMessage Message
		{
			get { return _message; }
		}

		public override Type MessageType
		{
			get { return typeof (TMessage); }
		}
	}
}