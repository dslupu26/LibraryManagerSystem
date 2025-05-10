using Common;
using Common.DomainModels;
using Common.Repositories;

namespace Bussiness.Generic
{
    public class GenericManagerFactory : IGenericManagerFactory
    {
        private ICurrentUserProvider currentUserProvider;
        private readonly IRepositoryFactory repositoryFactory;
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        public GenericManagerFactory(ICurrentUserProvider currentUserProvider, IUnitOfWorkFactory unitOfWorkFactory, IRepositoryFactory repositoryFactory)
        {
            ArgumentNullException.ThrowIfNull(currentUserProvider, nameof(currentUserProvider));
            ArgumentNullException.ThrowIfNull(unitOfWorkFactory, nameof(unitOfWorkFactory));
            ArgumentNullException.ThrowIfNull(repositoryFactory, nameof(repositoryFactory));

            this.currentUserProvider = currentUserProvider;
            this.repositoryFactory = repositoryFactory;
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        public IGenericManager<TDomainModel> GetNew<TDomainModel>() where TDomainModel : class, IDomainModel
        {
            return new GenericManager<TDomainModel>(this.currentUserProvider, this.unitOfWorkFactory, this.repositoryFactory);
        }
    }
}
