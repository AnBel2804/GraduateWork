using Graduate_Work.Data;
using Graduate_Work.Models;
using Graduate_Work.Repository.IRepository;

namespace Graduate_Work.Repository
{
    public class DepartmentRepository : Repository<Department>, IDepartmentRepository
    {
        private ApplicationDbContext _db;
        public DepartmentRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
