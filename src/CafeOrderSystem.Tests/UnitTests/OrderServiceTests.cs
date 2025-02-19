using CafeOrderSystem.Domain.Entities;
using CafeOrderSystem.Domain.Enums;
using CafeOrderSystem.Domain.Interfaces;
using CafeOrderSystem.Domain.Services;
using Moq;
using Xunit;

namespace CafeOrderSystem.Tests.UnitTests;

public class OrderServiceTests
{
    private readonly Mock<IOrderRepository> _mockRepo;
    private readonly OrderService _orderService;

    public OrderServiceTests()
    {
        _mockRepo = new Mock<IOrderRepository>();
        _orderService = new OrderService(_mockRepo.Object);
    }

    [Fact]
    public async Task CreateOrderAsync_ShouldAddOrderAndCalculateTotal()
    {
        // Arrange
        var order = new Order
        {
            CustomerName = "S7S",
            PaymentMethod = "Card",
            OrderItems = new List<OrderItem>
            {
                new OrderItem { ItemName = "Sandwich", Price = 10.0m },
                new OrderItem { ItemName = "Tea", Price = 5.0m }
            }
        };

        // Act
        await _orderService.CreateOrderAsync(order);

        // Assert
        _mockRepo.Verify(r => r.AddOrderAsync(order), Times.Once);
        Assert.Equal(15.0m, order.TotalAmount); // Verify business logic (total calculation)
    }

    [Fact]
    public async Task UpdateOrderStatusAsync_ValidTransition_UpdatesStatus()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var order = new Order
        {
            Id = orderId,
            CustomerName = "S7S",
            PaymentMethod = "Card",
            Status = OrderStatus.InProgress.ToString(),
            OrderItems = new List<OrderItem>()
        };

        _mockRepo.Setup(r => r.GetOrderByIdAsync(orderId))
            .ReturnsAsync(order);

        // Act
        await _orderService.UpdateOrderStatusAsync(orderId, OrderStatus.Completed.ToString());

        // Assert
        Assert.Equal("Completed", order.Status);
        _mockRepo.Verify(r => r.UpdateOrderAsync(order), Times.Once); // Verify repository is called
    }

    [Fact]
    public async Task UpdateOrderStatusAsync_InvalidTransition_ThrowsException()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var order = new Order
        {
            Id = orderId,
            CustomerName = "S7S",
            PaymentMethod = "Card",
            Status = OrderStatus.Completed.ToString(),
            OrderItems = new List<OrderItem>()
        };

        _mockRepo.Setup(r => r.GetOrderByIdAsync(orderId))
            .ReturnsAsync(order);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _orderService.UpdateOrderStatusAsync(orderId, OrderStatus.Canceled.ToString()));
    }

    [Fact]
    public async Task GetOrdersByStatusAndTimeRangeAsync_AppliesFilters()
    {
        // Arrange
        var orders = new List<Order>
        {
            new Order
            {
                CustomerName = "John Doe",
                PaymentMethod = "Cash",
                Status = OrderStatus.Completed.ToString(),
                OrderTime = DateTime.Parse("2023-01-01T12:00:00Z"),
                OrderItems = new List<OrderItem>
                {
                    new OrderItem { ItemName = "Coffee", Price = 5.0m }
                }
            },
            new Order
            {
                CustomerName = "Jane Doe",
                PaymentMethod = "Card",
                Status = OrderStatus.Completed.ToString(),
                OrderTime = DateTime.Parse("2023-02-01T12:00:00Z"),
                OrderItems = new List<OrderItem>
                {
                    new OrderItem { ItemName = "Sandwich", Price = 10.0m }
                }
            }
        };

        _mockRepo.Setup(r => r.GetOrdersByStatusAsync(OrderStatus.Completed.ToString()))
            .ReturnsAsync(orders);

        var from = DateTime.Parse("2023-01-15T00:00:00Z");
        var to = DateTime.Parse("2023-02-15T23:59:59Z");

        // Act
        var result = await _orderService.GetOrdersByStatusAndTimeRangeAsync(
            OrderStatus.Completed.ToString(), from, to);

        // Assert
        Assert.Single(result);
        _mockRepo.Verify(r => r.GetOrdersByStatusAsync(OrderStatus.Completed.ToString()), Times.Once);
    }

    [Fact]
    public async Task GetOrdersByStatusAsync_IncludesOrderItems()
    {
        // Arrange
        var order = new Order
        {
            CustomerName = "S7S",
            PaymentMethod = "Card",
            Status = OrderStatus.InProgress.ToString(),
            OrderItems = new List<OrderItem>
            {
                new OrderItem { ItemName = "Tea", Price = 5.0m }
            }
        };

        _mockRepo.Setup(r => r.GetOrdersByStatusAsync(OrderStatus.InProgress.ToString()))
            .ReturnsAsync(new List<Order> { order });

        // Act
        var result = await _orderService.GetOrdersByStatusAndTimeRangeAsync(
            OrderStatus.InProgress.ToString(), null, null);

        // Assert
        Assert.Single(result);
        Assert.Equal("Tea", result[0].OrderItems[0].ItemName);
        _mockRepo.Verify(r => r.GetOrdersByStatusAsync(OrderStatus.InProgress.ToString()), Times.Once);
    }
}