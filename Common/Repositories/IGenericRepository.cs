using Common.DomainModels;
using System.Linq.Expressions;

namespace Common.Repositories
{
    public interface IGenericRepository<T> where T : IDomainModel
    {
        public void Add(T model);

        public T Get(int id);

        public void Update(T model);

        public void Delete(T model);

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> criteriaExpression);

        public IEnumerable<T> GetIncluding(IEnumerable<Expression<Func<T, bool>>> criteriaList, List<Expression<Func<T, object>>> includeProperties);
        
        public bool Any(Expression<Func<T, bool>> expression);
    }
}
