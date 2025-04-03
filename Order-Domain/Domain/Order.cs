using Order_Domain.Domain.Enum;

namespace Order_Domain.Domain;
public class Order
{
    public Guid Id { get; set; }
    public OrderStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string? CustomerName { get; set; }
    public string CustomerEmail { get; set; }
    public string? ShippingAddress { get; set; }
    public decimal TotalAmount { get; set; }
}
