using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Order_Persistence;

public class GeneralRepository<TEntity> : IRepository<TEntity> where TEntity : class
{
    protected readonly OrderDBContext _dbContext;

    public GeneralRepository(OrderDBContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(TEntity entity)
    {
        await _dbContext.Set<TEntity>().AddAsync(entity);
    }

    public async Task<TEntity?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Set<TEntity>().FindAsync(id);
    }

    public void Update(TEntity entity)
    {
        _dbContext.Set<TEntity>().Update(entity);
    }

    public void Remove(TEntity entity)
    {
        _dbContext.Set<TEntity>().Remove(entity);
    }

    public async Task<List<TEntity>> GetAsync(Expression<Func<TEntity, bool>>? predicate = null)
    {
        var query = _dbContext.Set<TEntity>().AsQueryable();
        if (predicate != null)
            query = query.Where(predicate);

        return await query.ToListAsync();
    }

}
