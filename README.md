# Cafe Order Management API

## Overview
This is a RESTful API for managing cafe orders, built using **ASP.NET Core**, **Entity Framework Core (EF Core)**, and **PostgreSQL**. The API allows customers to create orders, manage order items, and track the status of their orders.

## Features
- Create new orders with customer details and payment method.
- Add order items to an order.
- Retrieve order details.
- Update order status.
- Persist data using **PostgreSQL**.
- Implements **Entity Framework Core** for database interactions.

## Technologies Used
- **ASP.NET Core 8.0**
- **Entity Framework Core**
- **PostgreSQL**
- **Dependency Injection**

## Getting Started

### Prerequisites
Ensure you have the following installed:
- .NET 8.0 SDK
- PostgreSQL
- Visual Studio / VS Code
- Postman (for testing API endpoints)

### Setup Instructions
1. **Clone the Repository**
   ```sh
   git clone https://github.com/anasmostafa23/TestAsptask
   cd TestAsptask
   ```

2. **Configure the Database**
   - Update the `appsettings.json` file with your PostgreSQL connection string:
     ```json
     "ConnectionStrings": {
       "DefaultConnection": "Host=localhost;Port=5432;Database=CafeDB;Username=your_user;Password=your_password"
     }
     ```

3. **Apply Migrations & Create Database**
   Run the following commands to apply EF Core migrations:
   ```sh
   dotnet ef database update
   ```

4. **Run the Application**
   ```sh
   dotnet run
   ```

5. **Access API Documentation**
   Open your browser and navigate to:
   - Swagger UI: [http://localhost:5000/swagger](http://localhost:5000/swagger)

## API Endpoints

### 1. Create Order
- **POST** `/api/orders`
- **Request Body:**
  ```json
  {
    "customerName": "John Doe",
    "totalAmount": 20.00,
    "paymentMethod": "Credit Card",
    "orderItems": [
      { "itemName": "Latte" }
    ]
  }
  ```

### 2. Get All Orders
- **GET** `/api/orders`

### 3. Get Order by ID
- **GET** `/api/orders/{id}`

### 4. Update Order Status
- **PUT** `/api/orders/{id}`
- **Request Body:**
  ```json
  { "status": "Completed" }
  ```


## Notes
- Ensure PostgreSQL is running before starting the API.
- Use Postman for testing endpoints.

