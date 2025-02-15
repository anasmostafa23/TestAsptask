using CafeOrderSystem.Domain.Entities;
using CafeOrderSystem.Domain.Interfaces;
using CafeOrderSystem.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace CafeOrderSystem.Data.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly CafeDbContext _context;

    public OrderRepository(CafeDbContext context)
    {
        _context = context;
    }

    public async Task AddOrderAsync(Order order)
    {
        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();
    }

    public async Task<Order?> GetOrderByIdAsync(Guid id)
    {
        return await _context.Orders
            .Include(o => o.OrderItems)
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task UpdateOrderAsync(Order order)
    {
        _context.Orders.Update(order);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Order>> GetOrdersByStatusAsync(string? status)
    {
        var query = _context.Orders.AsQueryable();

        if (!string.IsNullOrEmpty(status))
        {
            query = query.Where(o => o.Status == status);
        }

        return await query.ToListAsync();
    }
}