namespace Common.DomainModels
{
    public interface IOrderByExpression<TEntity> where TEntity : IDomainModel
    {
        /// <summary>The apply order by.</summary>
        /// <param name="query">The query.</param>
        /// <returns>The <see cref="IOrderedQueryable"/>.</returns>
        IOrderedQueryable<TEntity> ApplyOrderBy(IQueryable<TEntity> query);

        /// <summary>The apply then by.</summary>
        /// <param name="query">The query.</param>
        /// <returns>The <see cref="IOrderedQueryable"/>.</returns>
        IOrderedQueryable<TEntity> ApplyThenBy(IOrderedQueryable<TEntity> query);
    }
}
