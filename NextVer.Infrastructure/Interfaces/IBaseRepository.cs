using System.Linq.Expressions;


namespace NextVer.Infrastructure.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        Task<bool> Add(T entity);
        Task<bool> Delete(int id);
        Task<bool> Edit<TKey, TDto>(TKey id, TDto dto);
        IQueryable<TLinkEntity> GetLinkEntitiesFor<TEntity, TLinkEntity>(
             Expression<Func<TLinkEntity, bool>> filterExpression
         )
             where TEntity : class
             where TLinkEntity : class, new();

        void AddLinkEntity<TLinkEntity>(TLinkEntity entity) where TLinkEntity : class;
        void RemoveLinkEntity<TLinkEntity>(TLinkEntity entity) where TLinkEntity : class;
        Task<T> GetById(int id, params Expression<Func<T, object>>[] includes);
        Task<bool> Update(T entity);
        Task<IEnumerable<T>> GetAll();
        Task<bool> SaveChangesAsync();
        Task<bool> SaveChangesWithTransactionAsync();
        Task<int> GetNumberOfEntities<T>() where T : class;
        Task<IEnumerable<TEntity>> GetEntitiesBy<TEntity>(Expression<Func<TEntity, bool>> filterExpression)
            where TEntity : class;

    }
}