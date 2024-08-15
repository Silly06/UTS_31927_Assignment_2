using Microsoft.EntityFrameworkCore;
using PetPlayApp.Server.Db.Repos;

namespace PetPlayApp.Server.Db.Services
{
	public class RepositoryProvider
	{
		public RepositoryProvider(DatabaseContext databaseContext) 
		{ 
			this.databaseContext = databaseContext;
		}

		DatabaseContext databaseContext { get; set; }

		public Repository<T> GetRepository<T>() where T : class
		{
			return new Repository<T>(databaseContext);
		}
	}
}
