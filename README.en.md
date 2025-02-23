

# Café Order Management System

A RESTful API built with ASP.NET Core 8 and PostgreSQL for managing café orders. This project adheres to domain-driven design, SOLID principles, and Microsoft coding standards.

---

## Key Features

### 1. **Domain Rules Compliance**
- **Order Creation**:  
  Orders include `CustomerName`, `PaymentMethod`, and `OrderItems` with automatic `TotalAmount` calculation.  
  - Initial status is `InProgress`.  
  - `OrderItems` have a required `ItemName` and `Price`.  
- **Status Transitions**:  
  - Invalid transitions (e.g., canceling a completed order) throw `InvalidOperationException`.  
- **Order Retrieval**:  
  - Filter orders by status (`InProgress`, `Completed`, `Canceled`) and time range.

### 2. **API Compliance**
- **Endpoints**:  
  ```plaintext
  POST    /api/orders           → Create an order  
  PUT     /api/orders/{id}/status → Update order status  
  GET     /api/orders           → Retrieve orders by status and time range  
  ```
- **RESTful Design**:  
  - Proper HTTP verbs and status codes (`200 OK`, `400 Bad Request`, `404 Not Found`).  
  - Example:  
    ```bash
    PUT /api/orders/0b6a1e85-a683-4763-9196-4d608f48f6bd/status
    Body: "Completed"
    ```

### 3. **Entity Design**
- **Entities**:  
  - `Order`: Contains order metadata (e.g., `CustomerName`, `Status`, `TotalAmount`).  
  - `OrderItem`: Represents items in an order (one-to-many relationship with `Order`).  
- **Relationships**:  
  - `Order` ↔ `OrderItem` (one-to-many, configured via EF Core Fluent API).  
  ```csharp
  modelBuilder.Entity<Order>()
      .HasMany(o => o.OrderItems)
      .WithOne(oi => oi.Order)
      .HasForeignKey(oi => oi.OrderId);
  ```

### 4. **SOLID Principles**
- **Single Responsibility**:  
  - `OrderService` handles business logic (e.g., status transitions, total calculation).  
  - `OrderRepository` manages database operations.  
- **Dependency Inversion**:  
  - Interfaces (`IOrderRepository`) decouple layers.  
  - Dependency injection used throughout (e.g., `DbContext`, services).  
- **No God Classes**:  
  - Logic split into `API` (controllers), `Domain` (services), and `Data` (repositories).

### 5. **Dependency Management**
- **Lifetimes**:  
  - `DbContext` and repositories use **scoped** lifetimes (aligned with HTTP requests).  
  - No unnecessary singletons.  
- **Async Database Operations**:  
  - All repository methods use `async/await` (e.g., `AddOrderAsync`, `SaveChangesAsync`).

### 6. **Project Structure**
```
CafeOrderSystem/
├── API/           # Controllers, DTOs, configuration
├── Domain/        # Entities, enums, business logic (services)
├── Data/          # DbContext, repositories, migrations
└── Tests/         # Unit and integration tests
```

### 7. **Endpoints & Routing**
- **Routes**:  
  ```csharp
  [Route("api/[controller]")]
  public class OrdersController : ControllerBase
  ```
- **Examples**:  
  - `GET /api/orders?status=Completed&from=2023-01-01&to=2023-12-31`  
  - `POST /api/orders` with JSON body (see example below).

### 8. **Code Style**
- **CamelCase/PascalCase**:  
  - Properties (e.g., `CustomerName`, `TotalAmount`).  
  - Methods (e.g., `GetOrdersByStatusAsync`).  
- **Formatting**:  
  - Follows Microsoft .NET coding conventions.  
  - Clean indentation, no hardcoded strings.  

### 9. **Unit Tests**
- **Isolation**:  
  - Uses `Moq` to mock `IOrderRepository`.  
  - Tests focus on business logic, not database integration.  
- **Coverage**:  
  ```plaintext
  CreateOrderAsync_ShouldAddOrderAndCalculateTotal  
  UpdateOrderStatusAsync_InvalidTransition_ThrowsException  
  GetOrdersByStatusAndTimeRangeAsync_AppliesFilters  
  GetOrdersByStatusAsync_IncludesOrderItems  
  ```

---

## How to Run

1. **Prerequisites**:  
   - .NET 8 SDK  
   - PostgreSQL  

2. **Setup**:  
   ```bash
   git clone https://github.com/anasmostafa23/cafe-order-system.git
   cd cafe-order-system
   dotnet restore
   ```

3. **Database**:  
   Update connection string in `appsettings.json`, then run:  
   ```bash
   dotnet ef database update
   ```

4. **Run**:  
   ```bash
   dotnet run --project src/CafeOrderSystem.API
   ```

5. **Test**:  
   ```bash
   dotnet test
   ```

---

## Example Request

**Create an Order**:
```bash
POST /api/orders
Body:
{
  "customerName": "S7S",
  "paymentMethod": "Card",
  "orderItems": [
    { "itemName": "Sandwich", "price": 10.0 },
    { "itemName": "Tea", "price": 5.0 }
  ]
}
```

---

