using Common;
using Common.DomainModels;
using Common.Repositories;

namespace Repositories
{
    public class RepositoryFactory : IRepositoryFactory
    {
        private readonly ICurrentUserProvider userProvider;
        public RepositoryFactory(ICurrentUserProvider userProvider)
        { 
            this.userProvider = userProvider;
        }

        public IGenericRepository<TDomainModel> GetNew<TDomainModel>(IUnitOfWork unitOfWork) where TDomainModel : class, IDomainModel
        {
            return new GenericRepository<TDomainModel>(unitOfWork, this.userProvider); 
        }
    }
}
