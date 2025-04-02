using MediatR;
using Microsoft.AspNetCore.Mvc;
using Order_Application.Command;

namespace WebApplication_Order.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly ISender _sender;
    public OrderController(ISender sender)
    {
        _sender = sender;
    }
    [HttpPost]
    public async Task<IActionResult> Create(CreateOrderCommand request)
    {
        var result = await _sender.Send(request);
        return Ok(result);
    }
}

