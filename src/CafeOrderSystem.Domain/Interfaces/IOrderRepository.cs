using CafeOrderSystem.Domain.Entities;

namespace CafeOrderSystem.Domain.Interfaces;

public interface IOrderRepository
{
    Task AddOrderAsync(Order order);
    Task<Order?> GetOrderByIdAsync(Guid id);
    Task UpdateOrderAsync(Order order);
    Task<List<Order>> GetOrdersByStatusAsync(string? status);
}