using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Order_Application.Command;
using Order_Application.Query;
using Shared.Contracts.Event;

namespace WebApplication_Order.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly ISender _mediator;
    private readonly IPublishEndpoint _publishEndpoint;
    public OrderController(ISender mediator, IPublishEndpoint publishEndpoint)
    {
        _mediator = mediator;
        _publishEndpoint = publishEndpoint;
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var order = await _mediator.Send(new GetOrderByIdQuery() { OrderId = id });
        if (order == null)
            return NotFound();

        return Ok(order);
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAll()
    {
        var orders = await _mediator.Send(new GetOrdersQuery());
        return Ok(orders);
    }
    [HttpPost]
    public async Task<IActionResult> Create(CreateOrderCommand command)
    {
        var orderId = await _mediator.Send(command);
        var orderCreatedEvent = new OrderCreatedEvent(orderId, command.TotalAmount, command.CustomerEmail);

        await _publishEndpoint.Publish(orderCreatedEvent);
        return Ok(orderId);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateOrderCommand command)
    {
        if (id != command.OrderId) return BadRequest("ID mismatch");

        var success = await _mediator.Send(command);
        if (!success) return NotFound();

        return NoContent();
    }
}

