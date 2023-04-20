using Graduate_Work.Data;
using Graduate_Work.Repository.IRepository;

namespace Graduate_Work.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;
        public IIdentityUserRepository IdentityUser { get; private set; }
        public ICustomerRepository Customer { get; private set; }
        public IDepartmentRepository Department { get; private set; }
        public IPackageRepository Package { get; private set; }
        public IPackageTypeRepository PackageType { get; private set; }
        public ISenderInfoRepository SenderInfo { get; private set; }
        public IReciverInfoRepository ReciverInfo { get; private set; }
        public IRouteRepository Route { get; private set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            IdentityUser = new IdentityUserRepository(_db);
            Customer = new CustomerRepository(_db);
            Department = new DepartmentRepository(_db);
            Package = new PackageRepository(_db);
            PackageType = new PackageTypeRepository(_db);
            SenderInfo = new SenderInfoRepository(_db);
            ReciverInfo = new ReciverInfoRepository(_db);
            Route = new RouteRepository(_db);
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
