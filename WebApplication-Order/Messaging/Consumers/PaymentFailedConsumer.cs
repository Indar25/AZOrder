using MassTransit;
using MediatR;
using Order_Application.Command;
using Order_Application.Query;
using Order_Domain.Domain.Enum;
using Shared.Contracts.Event;

namespace WebApplication_Order.Messaging.Consumers;
public class PaymentFailedConsumer(ISender _mediator) : IConsumer<PaymentFailedEvent>
{
    public async Task Consume(ConsumeContext<PaymentFailedEvent> context)
    {
        var order = await _mediator.Send(new GetOrderByIdQuery() { OrderId = context.Message.OrderId });
        if (order is null)
            return;

        order.Status = OrderStatus.Cancelled;

        await _mediator.Send(new UpdateOrderCommand()
        {
            OrderId = order.Id,
            CustomerEmail = order.CustomerEmail,
            CustomerName = order.CustomerEmail,
            ShippingAddress = order.ShippingAddress,
            Status = order.Status,
            TotalAmount = order.TotalAmount
        });
    }
}

