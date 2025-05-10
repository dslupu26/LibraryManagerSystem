using Common.Repositories;
using Common;

namespace Bussiness
{
    public class DatabaseManager : IDatabaseManager
    {
        private ICurrentUserProvider currentUserProvider;
        private readonly IRepositoryFactory repositoryFactory;
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        public DatabaseManager(ICurrentUserProvider currentUserProvider, IUnitOfWorkFactory unitOfWorkFactory, IRepositoryFactory repositoryFactory)
        {
            ArgumentNullException.ThrowIfNull(currentUserProvider, nameof(currentUserProvider));
            ArgumentNullException.ThrowIfNull(unitOfWorkFactory, nameof(unitOfWorkFactory));
            ArgumentNullException.ThrowIfNull(repositoryFactory, nameof(repositoryFactory));

            this.currentUserProvider = currentUserProvider;
            this.repositoryFactory = repositoryFactory;
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        public void CreateTables()
        {           
            using (var unitOfWork = this.unitOfWorkFactory.GetNew())
            {
                unitOfWork.BeginTransaction();
                unitOfWork.ForceCreate();
                unitOfWork.CommitTransaction();
            }
        }

        public void DeleteTables()
        {

            using (var unitOfWork = this.unitOfWorkFactory.GetNew())
            {
                unitOfWork.BeginTransaction();
                unitOfWork.DropTable();
                unitOfWork.CommitTransaction();
            }
        }
    }
}
