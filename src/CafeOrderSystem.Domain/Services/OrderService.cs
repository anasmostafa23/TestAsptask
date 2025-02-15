using CafeOrderSystem.Domain.Entities;
using CafeOrderSystem.Domain.Enums;
using CafeOrderSystem.Domain.Interfaces;

namespace CafeOrderSystem.Domain.Services;

public class OrderService
{
    private readonly IOrderRepository _orderRepository;

    public OrderService(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task CreateOrderAsync(Order order)
    {
        if (order == null)
            throw new ArgumentException("Order data is required.");

        await _orderRepository.AddOrderAsync(order);
    }

    public async Task UpdateOrderStatusAsync(Guid orderId, string status)
    {
        var order = await _orderRepository.GetOrderByIdAsync(orderId);
        if (order == null)
            throw new ArgumentException("Order not found.");

        if ((order.Status == OrderStatus.Completed.ToString() && status == OrderStatus.Canceled.ToString()) ||
            (order.Status == OrderStatus.Canceled.ToString() && status == OrderStatus.Completed.ToString()))
            throw new InvalidOperationException("Invalid status transition.");

        order.Status = status;
        await _orderRepository.UpdateOrderAsync(order);
    }

    public async Task<List<Order>> GetOrdersByStatusAndTimeRangeAsync(string? status, DateTime? from, DateTime? to)
    {
        var orders = await _orderRepository.GetOrdersByStatusAsync(status);

        if (from != null && to != null)
        {
            orders = orders
                .Where(o => o.OrderTime >= from && o.OrderTime <= to)
                .ToList();
        }

        return orders;
    }
}