namespace AssetManagement.Tests
{
	using System;
	using System.Linq;
	using System.Reflection;
	using AssetManagement.Infrastructure;
	using AssetManagement.Infrastructure.Messaging;
	using Autofac;

	public abstract class Scenario
	{
		private readonly Exception _exception;

		protected Scenario()
		{
			using (var container = BuildContainerImpl())
			{
				var given = ResolveGivenAction(container);
				var when = ResolveWhenAction(container);
				var then = ResolveThenAction(container);

				given();
				try
				{
					when();
				}
				catch (Exception ex)
				{
					_exception = ex;
				}

				then();
			}
		}

		protected virtual void OverrideRegistrations(ContainerBuilder builder)
		{
		}

		protected Exception Exception
		{
			get { return _exception; }
		}

		private IContainer BuildContainerImpl()
		{
			var builder = new ContainerBuilder();
			builder.RegisterAssemblyModules(typeof(IBus).Assembly, typeof(Core.ThisAssembly).Assembly);

			OverrideRegistrations(builder);

			return builder.Build();
		}

		private Action ResolveGivenAction(IContainer container)
		{
			return ResolveMethodByAttribute<GivenAttribute>(container, true);
		}

		private Action ResolveWhenAction(IContainer container)
		{
			return ResolveMethodByAttribute<WhenAttribute>(container, false);
		}

		private Action ResolveThenAction(IContainer container)
		{
			return ResolveMethodByAttribute<ThenAttribute>(container, false);
		}

		private Action ResolveMethodByAttribute<T>(IContainer container, bool optional) where T : Attribute
		{
			var index = typeof (T).Name.IndexOf("Attribute", StringComparison.InvariantCultureIgnoreCase);
			var attributeName = typeof (T).Name.Substring(0, index);

			var type = GetType();
			var methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Where(methodInfo => CustomAttributeExtensions.GetCustomAttribute<T>(methodInfo) != null).ToArray();
			if (methods.Length == 0)
			{
				if (optional)
					return () => { };

				var message = string.Format("You should mark one method with {0} attribute", attributeName);
				throw new Exception(message);
			}
			else if (methods.Length > 1)
			{
				var message = string.Format("More than one method marked with {0} attribute", attributeName);
				throw new Exception(message);
			}

			var targetMethod = methods[0];
			var methodArgs = targetMethod.GetParameters().Select(parameter =>
			{
				var arg = container.ResolveOptional(parameter.ParameterType);
				if (arg == null)
				{
					var message = string.Format("{0} is not registered in IoC container", parameter.ParameterType);
					throw new Exception(message);
				}

				return arg;
			}).ToArray();

			return () => targetMethod.Invoke(this, methodArgs);
		}
	}
}