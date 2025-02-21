

```markdown
# Система управления заказами в кафе

RESTful API, разработанное на ASP.NET Core 8 и PostgreSQL для управления заказами в кафе. Проект соответствует принципам предметно-ориентированного проектирования (DDD), SOLID и стандартам кодирования Microsoft.

---

## Основные особенности

### 1. **Соблюдение правил предметной области**
- **Создание заказа**:  
  Заказы включают `CustomerName`, `PaymentMethod` и `OrderItems` с автоматическим расчетом `TotalAmount`.  
  - Начальный статус заказа — `InProgress`.  
  - `OrderItems` содержат обязательные поля `ItemName` и `Price`.  
- **Переходы статусов**:  
  - Недопустимые переходы (например, отмена завершенного заказа) вызывают исключение `InvalidOperationException`.  
- **Получение заказов**:  
  - Фильтрация заказов по статусу (`InProgress`, `Completed`, `Canceled`) и временному диапазону.

### 2. **Соответствие API**
- **Эндпоинты**:  
  ```plaintext
  POST    /api/orders           → Создать заказ  
  PUT     /api/orders/{id}/status → Обновить статус заказа  
  GET     /api/orders           → Получить заказы по статусу и временному диапазону  
  ```
- **RESTful-дизайн**:  
  - Использование правильных HTTP-методов и статусных кодов (`200 OK`, `400 Bad Request`, `404 Not Found`).  
  - Пример:  
    ```bash
    PUT /api/orders/0b6a1e85-a683-4763-9196-4d608f48f6bd/status
    Body: "Completed"
    ```

### 3. **Дизайн сущностей**
- **Сущности**:  
  - `Order`: Содержит метаданные заказа (например, `CustomerName`, `Status`, `TotalAmount`).  
  - `OrderItem`: Представляет элементы заказа (связь один-ко-многим с `Order`).  
- **Связи**:  
  - `Order` ↔ `OrderItem` (один-ко-многим, настроено через Fluent API EF Core).  
  ```csharp
  modelBuilder.Entity<Order>()
      .HasMany(o => o.OrderItems)
      .WithOne(oi => oi.Order)
      .HasForeignKey(oi => oi.OrderId);
  ```

### 4. **Принципы SOLID**
- **Единая ответственность**:  
  - `OrderService` обрабатывает бизнес-логику (например, переходы статусов, расчет суммы).  
  - `OrderRepository` управляет операциями с базой данных.  
- **Инверсия зависимостей**:  
  - Интерфейсы (`IOrderRepository`) разделяют слои.  
  - Везде используется внедрение зависимостей (например, `DbContext`, сервисы).  
- **Отсутствие "божественных классов"**:  
  - Логика разделена на `API` (контроллеры), `Domain` (сервисы) и `Data` (репозитории).

### 5. **Управление зависимостями**
- **Время жизни**:  
  - `DbContext` и репозитории используют **scoped** время жизни (соответствует HTTP-запросам).  
  - Нет лишних синглтонов.  
- **Асинхронные операции с базой данных**:  
  - Все методы репозиториев используют `async/await` (например, `AddOrderAsync`, `SaveChangesAsync`).

### 6. **Структура проекта**
```
CafeOrderSystem/
├── API/           # Контроллеры, DTO, конфигурация
├── Domain/        # Сущности, перечисления, бизнес-логика (сервисы)
├── Data/          # DbContext, репозитории, миграции
└── Tests/         # Модульные и интеграционные тесты
```

### 7. **Эндпоинты и маршрутизация**
- **Маршруты**:  
  ```csharp
  [Route("api/[controller]")]
  public class OrdersController : ControllerBase
  ```
- **Примеры**:  
  - `GET /api/orders?status=Completed&from=2023-01-01&to=2023-12-31`  
  - `POST /api/orders` с JSON-телом (см. пример ниже).

### 8. **Стиль кода**
- **CamelCase/PascalCase**:  
  - Свойства (например, `CustomerName`, `TotalAmount`).  
  - Методы (например, `GetOrdersByStatusAsync`).  
- **Форматирование**:  
  - Соответствует стандартам кодирования Microsoft .NET.  
  - Чистые отступы, отсутствие хардкодированных строк.  

### 9. **Модульные тесты**
- **Изоляция**:  
  - Используется `Moq` для мокирования `IOrderRepository`.  
  - Тесты сосредоточены на бизнес-логике, а не на интеграции с базой данных.  
- **Покрытие**:  
  ```plaintext
  CreateOrderAsync_ShouldAddOrderAndCalculateTotal  
  UpdateOrderStatusAsync_InvalidTransition_ThrowsException  
  GetOrdersByStatusAndTimeRangeAsync_AppliesFilters  
  GetOrdersByStatusAsync_IncludesOrderItems  
  ```

---

## Как запустить

1. **Предварительные требования**:  
   - .NET 8 SDK  
   - PostgreSQL  

2. **Настройка**:  
   ```bash
   git clone https://github.com/anasmostafa23/cafe-order-system.git
   cd cafe-order-system
   dotnet restore
   ```

3. **База данных**:  
   Обновите строку подключения в `appsettings.json`, затем выполните:  
   ```bash
   dotnet ef database update
   ```

4. **Запуск**:  
   ```bash
   dotnet run --project src/CafeOrderSystem.API
   ```

5. **Тестирование**:  
   ```bash
   dotnet test
   ```

---

## Пример запроса

**Создание заказа**:
```bash
POST /api/orders
Body:
{
  "customerName": "Иван Иванов",
  "paymentMethod": "Card",
  "orderItems": [
    { "itemName": "Сэндвич", "price": 10.0 },
    { "itemName": "Чай", "price": 5.0 }
  ]
}
```

---

```

This version is a direct translation of the original English README.md into Russian, ensuring that all technical terms and structure are preserved. You can now have a look on the English Version.