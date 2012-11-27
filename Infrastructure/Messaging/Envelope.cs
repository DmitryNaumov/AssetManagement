namespace AssetManagement.Infrastructure.Messaging
{
	using System;

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
			get { return typeof(TMessage); }
		}
	}
}