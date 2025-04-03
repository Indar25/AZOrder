using MediatR;
using Order_Domain.Domain;
using Order_Persistence;

namespace Order_Application.Query;
public record GetOrdersQuery : IRequest<List<Order>>;

public class GetOrdersQueryHandler(IUnitOfWork _unitOfWork) : IRequestHandler<GetOrdersQuery, List<Order>>
{
    public async Task<List<Order>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        var orders = await _unitOfWork.OrdersRepository.GetAsync();

        return orders;
    }
}
