using Graduate_Work.Data;
using Graduate_Work.Models;
using Graduate_Work.Repository.IRepository;

namespace Graduate_Work.Repository
{
    public class ReciverInfoRepository : Repository<ReciverInfo>, IReciverInfoRepository
    {
        private ApplicationDbContext _db;
        public ReciverInfoRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
