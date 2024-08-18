using PetPlayApp.Server.Db;
using PetPlayApp.Server.Services.Abstractions;


namespace PetPlayApp.Server.Services
{
	public class RepositoryProviderService : IRepositoryProviderService
	{
		public RepositoryProviderService(DatabaseContext databaseContext)
		{
			DatabaseContext = databaseContext;
		}

		DatabaseContext DatabaseContext { get; set; }

		public IRepository<T> GetRepository<T>() where T : class
		{
			return new Repository<T>(DatabaseContext);
		}
	}
}
