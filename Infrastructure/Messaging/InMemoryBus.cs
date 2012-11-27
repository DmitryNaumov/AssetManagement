namespace AssetManagement.Infrastructure.Messaging
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using System.Threading;
	using AssetManagement.Infrastructure.Persistence;
	using Autofac;
	using Autofac.Core;

	internal sealed class InMemoryBus : IBus
	{
		private readonly ILifetimeScope _lifetimeScope;
		private readonly IRepository _sagaRepository;

		private readonly Queue<Envelope> _queue = new Queue<Envelope>();

		public InMemoryBus(ILifetimeScope lifetimeScope, IRepository sagaRepository)
		{
			_lifetimeScope = lifetimeScope;
			_sagaRepository = sagaRepository;
		}

		public void Publish(object message, Action<ISendContext> contextCallback = null)
		{
			var envelope = (Envelope)GetType().GetMethod("WrapMessage", BindingFlags.NonPublic | BindingFlags.Instance).MakeGenericMethod(new[] {message.GetType()}).Invoke(this, new[] {message});

			if (contextCallback != null)
			{
				contextCallback(new SendContext(envelope));
			}

			lock (_queue)
			{
				_queue.Enqueue(envelope);
				if (_queue.Count > 1)
					return;
			}

			while (envelope != null)
			{
				try
				{
					GetType().GetMethod("PublishByType", BindingFlags.NonPublic | BindingFlags.Instance).MakeGenericMethod(new[] { envelope.MessageType }).Invoke(this, new[] { envelope });
				}
				catch (Exception ex)
				{
					// TODO: log exception
				}

				lock (_queue)
				{
					_queue.Dequeue();
					envelope = _queue.Count > 0 ? _queue.Peek() : null;
				}
			}
		}

		public bool IsIdle()
		{
			lock (_queue)
			{
				return _queue.Count == 0;
			}
		}

		private void PublishByType<T>(Envelope<T> envelope) where T : Message
		{
			foreach (var registration in _lifetimeScope.ComponentRegistry.Registrations)
			{
				var services = registration.Services.Cast<TypedService>().ToArray();

				foreach (var service in services)
				{
					if (typeof(IConsumer<T>).IsAssignableFrom(service.ServiceType))
					{
						if (services.Any(s => typeof(ISaga).IsAssignableFrom(s.ServiceType)))
						{
							InvokeSaga(registration, envelope);
						}
						else
						{
							InvokeHandler(registration, envelope);
						}

						break;
					}
				}
			}
		}

		private void InvokeHandler<T>(IComponentRegistration registration, Envelope<T> envelope) where T : Message
		{
			using (ReceiveContext.Create(this, envelope))
			{
				var message = envelope.Message;

				var consumer = (IConsumer<T>) registration.Activator.ActivateInstance(_lifetimeScope, Enumerable.Empty<Parameter>());
				consumer.Handle(message);
			}
		}

		private void InvokeSaga<T>(IComponentRegistration registration, Envelope<T> envelope) where T : Message
		{
			var initiatedBy = registration.Services.Cast<TypedService>().Single(service => service.ServiceType.IsGenericType && typeof(ISaga).IsAssignableFrom(service.ServiceType)).ServiceType;

			ISaga saga;
			if (initiatedBy.GetGenericArguments()[0] == typeof(T))
			{
				saga = (ISaga)registration.Activator.ActivateInstance(_lifetimeScope, Enumerable.Empty<Parameter>());
			}
			else
			{
				if (!envelope.CorrelationId.HasValue)
					return;

				saga = (ISaga)_sagaRepository.Get(envelope.CorrelationId.Value);
			}

			using (ReceiveContext.Create(this, envelope))
			{
				((IConsumer<T>) saga).Handle(envelope.Message);
				_sagaRepository.Save(saga.Id, saga);
			}
		}

		private Envelope WrapMessage<T>(T message) where T : Message
		{
			return new Envelope<T>(message);
		}
	}

	public static class ServiceBusExtensions
	{
		public static void Replay(this IBus bus, params object[] messages)
		{
			foreach (var message in messages)
			{
				bus.Publish(message);
			}

			SpinWait.SpinUntil(() => bus.IsIdle());
		}
	}
}
