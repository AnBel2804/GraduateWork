using Graduate_Work.Data;
using Graduate_Work.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Graduate_Work.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet;

        public Repository(ApplicationDbContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>();
        }

        public void Add(T item)
        {
            dbSet.Add(item);
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> paramFilter = null)
        {
            IQueryable<T> query = dbSet;
            if (paramFilter != null)
            {
                query = query.Where(paramFilter);
            }
            return query.ToList();
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>> paramFilter)
        {
            IQueryable<T> query = dbSet;
            query = query.Where(paramFilter);
            return query.FirstOrDefault();
        }

        public void Remove(T item)
        {
            dbSet.Remove(item);
        }

        public void RemoveRange(IEnumerable<T> item)
        {
            dbSet.RemoveRange(item);
        }

        public void Update(T item)
        {
            dbSet.Update(item);
        }
    }
}