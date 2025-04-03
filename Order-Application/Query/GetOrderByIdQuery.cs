using MediatR;
using Order_Domain.Domain;
using Order_Persistence;

namespace Order_Application.Query;
public record GetOrderByIdQuery : IRequest<Order>
{
    public Guid OrderId { get; set; }
}

public class GetOrderByIdQueryHandler(IUnitOfWork _unitOfWork) : IRequestHandler<GetOrderByIdQuery, Order>
{
    public async Task<Order> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var order = await _unitOfWork.OrdersRepository.GetByIdAsync(request.OrderId);

        return order;
    }
}

