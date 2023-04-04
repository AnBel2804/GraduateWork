using Graduate_Work.Data;
using Graduate_Work.Models;
using Graduate_Work.Repository.IRepository;

namespace Graduate_Work.Repository
{
    public class SenderInfoRepository : Repository<SenderInfo>, ISenderInfoRepository
    {
        private ApplicationDbContext _db;
        public SenderInfoRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
