namespace AssetManagement.Infrastructure
{
	using System;

	public interface IReceiveContext
	{
		Guid? CorrelationId { get; }
	}

	internal sealed class ReceiveContext : IReceiveContext
	{
		[ThreadStatic]
		private static ReceiveContext _current;

		private readonly IServiceBus _serviceBus;
		private readonly Envelope _envelope;

		public ReceiveContext(IServiceBus serviceBus, Envelope envelope)
		{
			_serviceBus = serviceBus;
			_envelope = envelope;
		}

		public static IDisposable Create(IServiceBus serviceBus, Envelope envelope)
		{
			if (_current != null)
				throw new InvalidOperationException();

			_current = new ReceiveContext(serviceBus, envelope);

			return new DisposeAction(() => _current = null);
		}

		public static ReceiveContext Current
		{
			get { return _current; }
		}

		public IServiceBus ServiceBus
		{
			get { return _serviceBus; }
		}

		public Guid? CorrelationId
		{
			get { return _envelope.CorrelationId; }
		}
	}
}