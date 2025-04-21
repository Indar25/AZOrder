namespace Order_Persistence;

public interface IUnitOfWork
{
    IRepository<T> GetRepository<T>() where T : class;
    Task<int> CommitAsync(CancellationToken cancellationToken = default);
}

public class UnitOfWork(OrderDBContext context) : IUnitOfWork
{
    private Dictionary<Type, object> _repositories = new();
    public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
            => await context.SaveChangesAsync(cancellationToken);

    public IRepository<T> GetRepository<T>() where T : class
    {
        var type = typeof(T);

        if (!_repositories.ContainsKey(type))
        {
            var repoInstnace = new GeneralRepository<T>(context);

            _repositories[type] = repoInstnace;
        }

        return (IRepository<T>)_repositories[type];
    }
}
