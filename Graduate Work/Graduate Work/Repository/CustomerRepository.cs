using Graduate_Work.Data;
using Graduate_Work.Models;
using Graduate_Work.Repository.IRepository;

namespace Graduate_Work.Repository
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        private ApplicationDbContext _db;
        public CustomerRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
