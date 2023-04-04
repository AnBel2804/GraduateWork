using Graduate_Work.Data;
using Graduate_Work.Repository.IRepository;

namespace Graduate_Work.Repository
{
    public class RouteRepository : Repository<Models.Route>, IRouteRepository
    {
        private ApplicationDbContext _db;
        public RouteRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
