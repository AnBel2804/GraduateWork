using Graduate_Work.Data;
using Graduate_Work.Models;
using Graduate_Work.Repository.IRepository;

namespace Graduate_Work.Repository
{
    public class PackageRepository : Repository<Package>, IPackageRepository
    {
        private ApplicationDbContext _db;
        public PackageRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
