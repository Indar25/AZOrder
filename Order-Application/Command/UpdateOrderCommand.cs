using FluentValidation;
using MediatR;
using Order_Domain.Domain;
using Order_Domain.Domain.Enum;
using Order_Persistence;

namespace Order_Application.Command;
public class UpdateOrderCommand : IRequest<bool>
{
    public Guid OrderId { get; set; }
    public string? CustomerName { get; set; }
    public string? CustomerEmail { get; set; }
    public string? ShippingAddress { get; set; }
    public decimal? TotalAmount { get; set; }
    public OrderStatus Status { get; set; }
}

public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateOrderCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _unitOfWork.GetRepository<Order>().GetByIdAsync(request.OrderId);
        if (order is null) return false;

        if (!string.IsNullOrWhiteSpace(request.CustomerName))
            order.CustomerName = request.CustomerName;
        if (!string.IsNullOrWhiteSpace(request.CustomerEmail))
            order.CustomerEmail = request.CustomerEmail;
        if (!string.IsNullOrWhiteSpace(request.ShippingAddress))
            order.ShippingAddress = request.ShippingAddress;
        if (request.TotalAmount.HasValue)
            order.TotalAmount = request.TotalAmount.Value;

        order.UpdatedAt = DateTime.UtcNow;
        _unitOfWork.GetRepository<Order>().Update(order);
        await _unitOfWork.CommitAsync(cancellationToken);
        return true;
    }
}
public class UpdateOrderCommandValidation : AbstractValidator<CreateOrderCommand>
{
    public UpdateOrderCommandValidation()
    {
        RuleFor(x => x.CustomerName).NotEmpty();
        RuleFor(x => x.ShippingAddress).NotEmpty();
        RuleFor(x => x.CustomerEmail).NotEmpty();
        RuleFor(x => x.TotalAmount).GreaterThan(10);
    }
}