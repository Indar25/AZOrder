namespace Shared.Contracts.Event;
public record OrderCreatedEvent(Guid OrderId, decimal Amount, string CustomerEmail);
public record PaymentSucceededEvent(Guid OrderId);
public record PaymentFailedEvent(Guid OrderId, string Reason);
