using Common.DomainModels;

namespace Common.Repositories
{
    public interface IRepositoryFactory
    {
        public IGenericRepository<TDomainModel> GetNew<TDomainModel>(IUnitOfWork unitOfWork) where TDomainModel : class, IDomainModel;
    }
}
