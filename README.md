The **Order API** is a .NET 8 Web API that handles customer orders as part of a distributed system. It is responsible for:

- Creating, updating, and retrieving orders  
- Publishing domain events (e.g., `OrderCreatedEvent`) to RabbitMQ using MassTransit  
- Participating in the **Saga Pattern (Choreography-based)** to coordinate workflows with other services  

---

### 🔧 Tech Stack:

- .NET 8  
- Clean Architecture  
- MediatR (CQRS)  
- Entity Framework Core  
- MassTransit + RabbitMQ  
- Application Insights (optional)  

---

### 🧩 Responsibilities:

- Receives order creation requests via HTTP  
- Persists order data to the database  
- Publishes `OrderCreatedEvent` to notify downstream services (e.g., Payment)  
- Listens to `PaymentSucceededEvent` / `PaymentFailedEvent` to update order status  
