using Business.Dtos;
using Common.DomainModels;
using System.Linq.Expressions;

namespace Business.Generic
{
    public interface IGenericDtoManager<T, TDto> where T : class, IDomainModel where TDto : BaseDto
    {
        IEnumerable<TDto> GetAllItems(params Expression<Func<T, object>>[] includeProperties);
        IEnumerable<TDto> GetAllItems(IEnumerable<Expression<Func<T, bool>>> criteriaList, params Expression<Func<T, object>>[] includeProperties);

        void Update(TDto dtoModel);

        TDto Add(TDto modelDto);

        TDto GetById(int id);

        void Delete(int id);
    }
}
