namespace Graduate_Work.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IIdentityUserRepository IdentityUser { get; }
        ICustomerRepository Customer { get; }
        IDepartmentRepository Department { get; }
        IPackageRepository Package { get; }
        IPackageTypeRepository PackageType { get; }
        ISenderInfoRepository SenderInfo { get; }
        IReciverInfoRepository ReciverInfo { get; }
        IRouteRepository RouteRepository { get; }
        void Save();
    }
}
