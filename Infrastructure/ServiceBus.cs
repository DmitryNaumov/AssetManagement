namespace AssetManagement.Infrastructure
{
	using System;
	using System.Collections;
	using System.Linq;
	using System.Reflection;
	using System.Threading;
	using AssetManagement.Infrastructure.Persistence;
	using Autofac;
	using Autofac.Core;

	internal sealed class ServiceBus : IServiceBus
	{
		private static readonly MethodInfo Dispatcher = typeof(ServiceBus).GetMethod("DeliverMessage", BindingFlags.Instance | BindingFlags.NonPublic);

		private readonly ILifetimeScope _lifetimeScope;
		private readonly IRepository _sagaRepository;

		private readonly Queue _queue = new Queue();

		public ServiceBus(ILifetimeScope lifetimeScope, IRepository sagaRepository)
		{
			_lifetimeScope = lifetimeScope;
			_sagaRepository = sagaRepository;
		}

		public void Publish(object message)
		{
			lock (_queue)
			{
				_queue.Enqueue(message);
				if (_queue.Count > 1)
					return;
			}

			while (message != null)
			{
				try
				{
					var method = Dispatcher.MakeGenericMethod(message.GetType());
					method.Invoke(this, new[] {message});
				}
				catch (Exception ex)
				{
					// TODO: log exception
				}

				lock (_queue)
				{
					_queue.Dequeue();
					message = _queue.Count > 0 ? _queue.Peek() : null;
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

		private void DeliverMessage<T>(T message)
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
							InvokeSaga(registration, message);
						}
						else
						{
							InvokeHandler(registration, message);
						}

						break;
					}
				}
			}
		}

		private void InvokeHandler<T>(IComponentRegistration registration, T message)
		{
			var consumer = (IConsumer<T>)registration.Activator.ActivateInstance(_lifetimeScope, Enumerable.Empty<Parameter>());
			consumer.Handle(message);
		}

		private void InvokeSaga<T>(IComponentRegistration registration, T message)
		{
			var correlatedBy = (CorrelatedBy)message;
			var initiatedBy = registration.Services.Cast<TypedService>().Single(service => service.ServiceType.IsGenericType && typeof(ISaga).IsAssignableFrom(service.ServiceType)).ServiceType;

			ISaga saga;
			if (initiatedBy.GetGenericArguments()[0] == typeof(T))
			{
				saga = (ISaga)registration.Activator.ActivateInstance(_lifetimeScope, new [] { new TypedParameter(typeof(Guid), correlatedBy.CorrelationId) });
			}
			else
			{
				saga = (ISaga)_sagaRepository.Get(correlatedBy.CorrelationId);
			}

			((IConsumer<T>) saga).Handle(message);
			_sagaRepository.Save(saga.Id, saga);
		}
	}

	public static class ServiceBusExtensions
	{
		public static void Replay(this IServiceBus serviceBus, params object[] messages)
		{
			foreach (var message in messages)
			{
				serviceBus.Publish(message);
			}

			SpinWait.SpinUntil(() => serviceBus.IsIdle());
		}
	}
}
