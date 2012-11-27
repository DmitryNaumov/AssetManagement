namespace AssetManagement.Infrastructure.Utilities
{
	using System;

	internal sealed class DisposeAction : IDisposable
	{
		private Action _action;

		public DisposeAction(Action action)
		{
			_action = action;
		}

		public void Dispose()
		{
			if (_action != null)
			{
				_action();
				_action = null;
			}
		}
	}
}