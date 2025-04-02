using Order_Domain.Domain;

namespace Order_Persistence;

public interface IUnitOfWork
{
    IRepository<Order> OrdersRepository { get; }
    Task<int> CommitAsync(CancellationToken cancellationToken = default);
}

public class UnitOfWork(OrderDBContext context) : IUnitOfWork
{
    private IRepository<Order> _ordersRepository;

    public async Task<int> CommitAsync(CancellationToken cancellationToken = default)
            => await context.SaveChangesAsync(cancellationToken);

    public IRepository<Order> OrdersRepository => _ordersRepository ?? (_ordersRepository = new GeneralRepository<Order>(context));
}
