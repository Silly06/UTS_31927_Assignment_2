using System.Linq.Expressions;

#nullable enable

namespace PetPlayApp.Server.Db
{
	public interface IRepository<T> where T : class
	{
		T? GetById(Guid id);
		IEnumerable<T> GetAll();
		IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
		void Add(T entity);
		void Remove(T entity);
		void Update(T entity);
		void RemoveAll();
	}
}
