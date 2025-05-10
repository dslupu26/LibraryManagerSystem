using Common;
using Common.DomainModels;
using Common.Repositories;

namespace Bussiness.Generic
{
    public class GenericManager<T> : IGenericManager<T> where T : class, IDomainModel
    {
        private ICurrentUserProvider currentUserProvider;
        private readonly IRepositoryFactory repositoryFactory;
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        public GenericManager(ICurrentUserProvider currentUserProvider, IUnitOfWorkFactory unitOfWorkFactory, IRepositoryFactory repositoryFactory)
        {
            ArgumentNullException.ThrowIfNull(currentUserProvider, nameof(currentUserProvider));
            ArgumentNullException.ThrowIfNull(unitOfWorkFactory, nameof(unitOfWorkFactory));
            ArgumentNullException.ThrowIfNull(repositoryFactory, nameof(repositoryFactory));

            this.currentUserProvider = currentUserProvider;
            this.repositoryFactory = repositoryFactory;
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        public IEnumerable<T> GetAll()
        {
            using (var unitOfWork = this.unitOfWorkFactory.GetNew())
            {
                var repo = repositoryFactory.GetNew<T>(unitOfWork);

                return repo.GetAll(null).ToList();
            }
        }

        protected virtual void BeforeDelete(IUnitOfWork unitOfWork,T model)
        {
        }

        protected virtual void AfterDelete(IUnitOfWork unitOfWork,T model)
        {
        }

        public void Delete(int id)
        {
            using (var unitOfWork = this.unitOfWorkFactory.GetNew())
            {
                unitOfWork.BeginTransaction();
                this.Delete(unitOfWork, id);
                unitOfWork.CommitTransaction();
            }
        }

        public void Delete(IUnitOfWork unitOfWork, int id)
        {
            ArgumentNullException.ThrowIfNull(unitOfWork, nameof(unitOfWork));
            var repo = repositoryFactory.GetNew<T>(unitOfWork);
            var domainModel = repo.Get(id);

            BeforeDelete(unitOfWork, domainModel);
            repo.Delete(domainModel);
            AfterDelete(unitOfWork, domainModel);

            unitOfWork.SaveChanges();
        }

        public T GetById(int id)
        {
            using (var unitOfWork = this.unitOfWorkFactory.GetNew())
            {
                var repo = repositoryFactory.GetNew<T>(unitOfWork);
                var domainModel = repo.Get(id);
                return domainModel;
            }
        }

        protected T GetById(int id, IUnitOfWork unitOfWork)
        {
            var repo = repositoryFactory.GetNew<T>(unitOfWork);
            var domainModel = repo.Get(id);
            return domainModel;
        }
    }
}
