using Common.DomainModels;
using Common.Repositories;

namespace Bussiness.Generic
{
    public interface IGenericManager<T> where T : class, IDomainModel
    {
        IEnumerable<T> GetAll();

        public void Delete(IUnitOfWork unitOfWork, int id);

        void Delete(int id);

        T GetById(int id);
    }
}
