using System.Linq.Expressions;

namespace PetPlayApp.Server.Db.Repos
{
    public interface IRepository<T> where T : class
    {
        T? GetById(Guid id);
        IEnumerable<T> GetAll();
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
        void Add(T entity);
        void Remove(T entity);
    }
}
