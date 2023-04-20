using System.Linq.Expressions;

namespace Graduate_Work.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        T GetFirstOrDefault(Expression<Func<T, bool>> paramFilter, params string[] prop);
        IEnumerable<T> GetAll(Expression<Func<T, bool>>? paramFilter = null, params string[] prop);
        void Add(T item);
        void Remove(T item);
        void RemoveRange(IEnumerable<T> item);
        void Update(T item);
    }
}
