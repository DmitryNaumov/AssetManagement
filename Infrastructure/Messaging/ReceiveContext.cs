namespace AssetManagement.Infrastructure.Messaging
{
	using System;
	using AssetManagement.Infrastructure.Utilities;

	internal sealed class ReceiveContext : IReceiveContext
	{
		[ThreadStatic]
		private static ReceiveContext _current;

		private readonly IBus _bus;
		private readonly Envelope _envelope;

		public ReceiveContext(IBus bus, Envelope envelope)
		{
			_bus = bus;
			_envelope = envelope;
		}

		public static IDisposable Create(IBus bus, Envelope envelope)
		{
			if (_current != null)
				throw new InvalidOperationException();

			_current = new ReceiveContext(bus, envelope);

			return new DisposeAction(() => _current = null);
		}

		public static ReceiveContext Current
		{
			get { return _current; }
		}

		public IBus Bus
		{
			get { return _bus; }
		}

		public Guid? CorrelationId
		{
			get { return _envelope.CorrelationId; }
		}
	}
}