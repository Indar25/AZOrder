# 🛒 Order API

The **Order API** is a .NET 8 Web API that handles customer orders as part of a distributed system. It communicates with the Payment API using RabbitMQ and participates in the **Saga Pattern (Choreography-based)**.

---

### 🔧 Tech Stack:

- .NET 8  
- Clean Architecture  
- MediatR (CQRS)  
- Entity Framework Core  
- PostgreSQL  
- MassTransit + RabbitMQ  
- Application Insights (optional)  

---

### 🧩 Responsibilities:

- Receives order creation requests via HTTP  
- Persists order data to PostgreSQL  
- Publishes `OrderCreatedEvent` to notify downstream services (e.g., Payment)  
- Listens to `PaymentSucceededEvent` / `PaymentFailedEvent` to update order status  

---

### 🔄 Saga Integration:

This service participates in a **Saga Choreography** flow by initiating the process and reacting to follow-up events from other services. It is the entry point of the business transaction.

---

### 🚀 Event Flow:

1. Client sends HTTP request to create an order  
2. Order API saves the order and publishes `OrderCreatedEvent`  
3. Payment API consumes the event and processes payment  
4. Order API listens for:  
   - `PaymentSucceededEvent` → marks order as Confirmed  
   - `PaymentFailedEvent` → marks order as Cancelled  

---

### 🐇 Running RabbitMQ Locally (Docker)

Use the following command to spin up RabbitMQ locally with the management UI:

```bash
docker run -d \
  --hostname rabbit-local \
  --name rabbitmq \
  -p 5672:5672 \
  -p 15672:15672 \
  -e RABBITMQ_DEFAULT_USER=guest \
  -e RABBITMQ_DEFAULT_PASS=guest \
  rabbitmq:3-management
```

- Visit the UI at: [http://localhost:15672](http://localhost:15672)  
- Login: `guest` / `guest`

---

# 💳 Payment API

The **Payment API** is a .NET 8 Web API designed to handle payment processing in a distributed microservices system. It communicates with the Order API using RabbitMQ and participates in the **Saga Pattern (Choreography-based)**.

---

### 🔧 Tech Stack:

- .NET 8  
- Clean Architecture  
- MediatR (CQRS)  
- Entity Framework Core  
- PostgreSQL  
- MassTransit + RabbitMQ  

---

### 🧩 Responsibilities:

- Listens for `OrderCreatedEvent` from RabbitMQ  
- Executes payment logic via command handler  
- Persists payment data to PostgreSQL  
- Publishes either:  
  - `PaymentSucceededEvent` (if payment is successful)  
  - `PaymentFailedEvent` (if something goes wrong)  

---

### 🔄 Saga Integration:

This service participates in a **Saga Choreography** flow by reacting to events instead of relying on a central orchestrator. It operates autonomously but contributes to a larger business transaction flow.

---

### 🚀 Event Flow:

1. `OrderCreatedEvent` is published by Order API  
2. Payment API consumes the event and processes the payment  
3. Payment API publishes either:  
   - `PaymentSucceededEvent` → triggers Order confirmation  
   - `PaymentFailedEvent` → triggers Order cancellation  

---

### 🐇 Running RabbitMQ Locally (Docker)

Use the following command to spin up RabbitMQ locally with the management UI:

```bash
docker run -d \
  --hostname rabbit-local \
  --name rabbitmq \
  -p 5672:5672 \
  -p 15672:15672 \
  -e RABBITMQ_DEFAULT_USER=guest \
  -e RABBITMQ_DEFAULT_PASS=guest \
  rabbitmq:3-management
```

- Visit the UI at: [http://localhost:15672](http://localhost:15672)  
- Login: `guest` / `guest`

---
### 🧠 What is the Saga Pattern?
The Saga Pattern is a design pattern used to manage distributed transactions across multiple microservices without using a traditional ACID (atomic) transaction.

Instead of locking resources across services (which doesn't scale), a Saga breaks the process into a sequence of local transactions, each followed by a compensating action (in case something goes wrong).

---
### ✅Why Use the Saga Pattern?
🚀 Because in microservices:
Distributed transactions are hard (no shared DB, no 2-phase commit)

You need to keep data eventually consistent

Services should be loosely coupled

---
### ✨ Benefits:
Enables resilient, scalable workflows

Maintains consistency across services

Allows for rollback using compensating actions

Encourages event-driven architecture

---
### 📍 When to Use the Saga Pattern?
You should consider using the Saga Pattern when:

✅ Use it when:
You have long-running transactions across multiple services

You need eventual consistency (not immediate consistency)

You want to avoid using distributed locks or 2PC

Your business process can be broken into steps

---
### ❌ Avoid when:
You need strong consistency immediately (e.g., banking core balances)

The process is short-lived and can be handled in a single service

Operations cannot be rolled back (and no compensation is possible)
---
### 🧩 Example Scenario
Let's say you have an e-commerce flow:

Create Order

Reserve Inventory

Charge Payment

With the Saga Pattern:

Each step is a local transaction

If Step 3 fails (payment), you compensate Step 2 (release inventory), and maybe cancel the order

---
### 🧱 Types of Sagas
Type	Description
Orchestration	One central service tells others what to do
Choreography	Services react to events and coordinate via messaging (MassTransit supports this well)

---
### ✅ Summary: What / Why / When
---
### What?	
A pattern for managing distributed workflows with local transactions and compensation
### Why?	
Ensures consistency across microservices without using distributed transactions
### When?	
Use when your process spans multiple services and can tolerate eventual consistency

---
