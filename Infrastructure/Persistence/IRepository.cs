namespace AssetManagement.Infrastructure.Persistence
{
	using System;

	public interface IRepository
	{
		object Get(Guid key);

		void Save(Guid key, object state);
	}
}
