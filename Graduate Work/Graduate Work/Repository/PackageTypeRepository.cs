using Graduate_Work.Data;
using Graduate_Work.Models;
using Graduate_Work.Repository.IRepository;

namespace Graduate_Work.Repository
{
    public class PackageTypeRepository : Repository<PackageType>, IPackageTypeRepository
    {
        private ApplicationDbContext _db;
        public PackageTypeRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
