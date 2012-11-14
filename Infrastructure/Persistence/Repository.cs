namespace AssetManagement.Infrastructure.Persistence
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Runtime.Serialization.Formatters.Binary;

	internal sealed class Repository : IRepository
	{
		private readonly Dictionary<Guid, object> _storage = new Dictionary<Guid, object>();

		public object Get(Guid key)
		{
			lock (_storage)
			{
				object value;
				if (!_storage.TryGetValue(key, out value))
				{
					return null;
				}

				return Clone(value);
			}
		}

		public void Save(Guid key, object newValue)
		{
			lock (_storage)
			{
				object value;
				if (_storage.TryGetValue(key, out value))
				{
					var concurrencyAware = newValue as IOptimisticConcurrencyAware;
					if (concurrencyAware != null)
					{
						if (concurrencyAware.Version != ((IOptimisticConcurrencyAware)value).Version)
							throw new OptimisticConcurrencyException();

						concurrencyAware.Version++;
					}
				}

				_storage[key] = Clone(newValue);
			}
		}

		private object Clone(object value)
		{
			using (var stream = new MemoryStream())
			{
				var formatter = new BinaryFormatter();
				formatter.Serialize(stream, value);

				stream.Position = 0;
				return formatter.Deserialize(stream);
			}
		}
	}
}