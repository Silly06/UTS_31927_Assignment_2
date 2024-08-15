using System.Linq.Expressions;

#nullable enable

namespace PetPlayApp.Server.Db.Repos
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly DatabaseContext _context; // Database context instance

        public Repository(DatabaseContext context)
        {
            _context = context; // Injected database context
        }

        public T GetById(Guid id)
        {
            return _context.Set<T>().Find(id); // Find entity by ID
        }

        public IEnumerable<T> GetAll()
        {
            return _context.Set<T>().ToList(); // Retrieve all entities
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().Where(predicate).ToList(); // Find entities by predicate
        }

        public void Add(T entity)
        {
            _context.Set<T>().Add(entity); // Add entity to DbSet
        }

        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public void Remove(T entity)
        {
            _context.Set<T>().Remove(entity); // Remove entity from DbSet
        }

		public void RemoveAll()
		{
			var entities = GetAll();
			foreach (var entity in entities)
			{
				_context.Remove(entity);
			}
			SaveChanges();
		}

		public void SaveChanges()
        {
            _context.SaveChanges(); // Save changes to the database
        }
    }
}
