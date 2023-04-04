using Graduate_Work.Data;
using Graduate_Work.Repository.IRepository;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Graduate_Work.Repository
{
    public class IdentityUserRepository : Repository<IdentityUser>, IIdentityUserRepository
    {
        private ApplicationDbContext _db;
        public IdentityUserRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
