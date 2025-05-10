using Common;
using Common.DomainModels;
using Common.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class, IDomainModel
    {
        private readonly DbContext context;
        private readonly ICurrentUserProvider userProvider;

        public GenericRepository(IUnitOfWork unitOfWork,ICurrentUserProvider userProvider)
        {
            ArgumentNullException.ThrowIfNull(unitOfWork, nameof(unitOfWork));
            ArgumentNullException.ThrowIfNull(userProvider, nameof(userProvider));

            var tmpcontext = unitOfWork as DbContext;
            if (tmpcontext == null)
            {
                throw new ArgumentNullException(nameof(tmpcontext));                
            }
            context = tmpcontext;

            this.userProvider = userProvider;
        }

        public void Add(T model) 
        {
            ArgumentNullException.ThrowIfNull(model, nameof(model));

            model.CreatedBy = this.userProvider.CurrentUser;
            model.CreatedOn = DateTime.Now;
            this.context.Set<T>().Attach(model);
            this.context.Entry(model).State = EntityState.Added;
        }

        public T Get(int id)
        {
            var model = context.Set<T>().AsNoTracking().FirstOrDefault(it => it.Id == id);
            return model;
        }

        public void Update(T model)
        {
            model.ModifiedBy = this.userProvider.CurrentUser;
            model.ModifiedOn = DateTime.Now;
            this.UpdatePrivate(model);            
        }

        private void UpdatePrivate(T entity)
        {
            T entityToBeUpdated;
            var localEntity = this.GetLocalEntityIfExists(entity.Id);
            if (localEntity != null)
            {
                entityToBeUpdated = localEntity;
            }
            else
            {
                entityToBeUpdated = entity;
            }

            this.context.Entry(entityToBeUpdated).State = EntityState.Modified;
        }

        private T GetLocalEntityIfExists(int idLocal)
        {
            var local = this.context.Set<T>().Local.FirstOrDefault(f => f.Id == idLocal);

            return local;
        }

        public void Delete(T model)
        {
            context.Remove(model);
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> criteriaExpression)
        {
            var query = this.context.Set<T>().AsNoTracking().AsQueryable();

            if (criteriaExpression != null)
            {
                query = query.Where(criteriaExpression).AsNoTracking().AsQueryable();
            }

            return query.AsEnumerable();

        }

        public bool Any(Expression<Func<T, bool>> expression)
        {
            var query = this.context.Set<T>().AsNoTracking().AsQueryable();

            if (expression != null)
            {
                return query.Any(expression);
            }
            return query.Any();
        }

        public IEnumerable<T> GetIncluding(IEnumerable<Expression<Func<T, bool>>> criteriaList, List<Expression<Func<T, object>>> includeProperties)
        {
            var databaseSet = this.context.Set<T>();

            var query = databaseSet.AsNoTracking();
             
            if (includeProperties != null)
            {
                query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            }

            if (criteriaList != null)
            {
                foreach (var criteria in criteriaList)
                {
                    query = query.Where(criteria).AsQueryable();
                }
            }

            return query.AsQueryable().AsEnumerable();
        }

        protected IQueryable<TEntity> ApplyOrderBy<TEntity>(IQueryable<TEntity> query, params IOrderByExpression<TEntity>[] orderByExpressions) where TEntity : IDomainModel
        {
            if (orderByExpressions == null)
            {
                return query;
            }

            IOrderedQueryable<TEntity> output = null;

            foreach (var orderByExpression in orderByExpressions)
            {
                if (output == null)
                {
                    output = orderByExpression.ApplyOrderBy(query);
                }
                else
                {
                    output = orderByExpression.ApplyThenBy(output);
                }
            }

            return output ?? query;
        }

    }
}