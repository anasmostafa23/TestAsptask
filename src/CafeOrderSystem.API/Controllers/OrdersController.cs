using CafeOrderSystem.Domain.Entities;
using CafeOrderSystem.Domain.Enums;
using CafeOrderSystem.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace CafeOrderSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly OrderService _orderService;

    // Inject OrderService via constructor
    public OrdersController(OrderService orderService)
    {
        _orderService = orderService;
    }

    // Create a new order
    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] Order order)
    {
        if (order == null)
        {
            return BadRequest("Order data is required.");
        }

        try
        {
            order.Status = OrderStatus.InProgress.ToString(); // Set initial status
            await _orderService.CreateOrderAsync(order);
            return Ok(order);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while creating the order.");
        }
    }

    // Update the status of an existing order
    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateOrderStatus(Guid id, [FromBody] OrderStatus status)
    {
        try
        {
            await _orderService.UpdateOrderStatusAsync(id, status.ToString());
            return Ok();
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message); // Invalid status transition
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while updating the order status.");
        }
    }

    // Get orders by status and time range
    [HttpGet]
    public async Task<IActionResult> GetOrders(
        [FromQuery] OrderStatus? status,
        [FromQuery] DateTime? from,
        [FromQuery] DateTime? to)
    {
        try
        {
            var orders = await _orderService.GetOrdersByStatusAndTimeRangeAsync(status?.ToString(), from, to);
            return Ok(orders);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while retrieving orders.");
        }
    }
}