using System.Linq.Expressions;

namespace Order_Persistence;
public interface IRepository<TEntity> where TEntity : class
{
    Task AddAsync(TEntity order);
    Task<TEntity?> GetByIdAsync(Guid id);
    Task<List<TEntity>> GetAsync(Expression<Func<TEntity, bool>>? predicate = null);
    void Update(TEntity order);
    void Remove(TEntity order);
}

