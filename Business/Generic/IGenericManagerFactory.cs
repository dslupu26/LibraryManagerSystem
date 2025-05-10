using Common.DomainModels;

namespace Bussiness.Generic
{
    public interface IGenericManagerFactory
    {
        public IGenericManager<TDomainModel> GetNew<TDomainModel>() where TDomainModel : class, IDomainModel;
    }
}
