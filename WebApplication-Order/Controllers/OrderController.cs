using MediatR;
using Microsoft.AspNetCore.Mvc;
using Order_Application.Command;
using Order_Application.Query;

namespace WebApplication_Order.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly ISender _mediator;
    public OrderController(ISender mediator)
    {
        _mediator = mediator;
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
    public async Task<IActionResult> GetAll()
    {
        var orders = await _mediator.Send(new GetOrdersQuery());
        return Ok(orders);
    }
    [HttpPost]
    public async Task<IActionResult> Create(CreateOrderCommand request)
    {
        var result = await _mediator.Send(request);
        return Ok(result);
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

