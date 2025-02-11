using CafeOrderSystem.Domain.Entities;
using CafeOrderSystem.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CafeOrderSystem.Data.Context;


namespace CafeOrderSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly CafeDbContext _context;

    public OrdersController(CafeDbContext context) => _context = context;

    [HttpPost]
    public async Task<IActionResult> CreateOrder(Order order)
    {
        order.Status = OrderStatus.InProgress.ToString();
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
        return Ok(order);
    }

    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] OrderStatus status)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order == null) return NotFound();

        if ((order.Status == OrderStatus.Completed.ToString() && status == OrderStatus.Canceled) ||
            (order.Status == OrderStatus.Canceled.ToString() && status == OrderStatus.Completed))
            return BadRequest("Invalid status transition");

        order.Status = status.ToString();
        await _context.SaveChangesAsync();
        return Ok(order);
    }

    [HttpGet]
    public async Task<IActionResult> GetOrders(
        [FromQuery] OrderStatus? status,
        [FromQuery] DateTime? from,
        [FromQuery] DateTime? to)
    {
        var query = _context.Orders.AsQueryable();

        if (status != null)
            query = query.Where(o => o.Status == status.ToString());

        if (from != null && to != null)
            query = query.Where(o => o.OrderTime >= from && o.OrderTime <= to);

        var orders = await query.ToListAsync();
        return Ok(orders);
    }
}