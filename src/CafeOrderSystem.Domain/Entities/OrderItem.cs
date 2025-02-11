namespace CafeOrderSystem.Domain.Entities;
public class OrderItem
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string ItemName { get; set; }
    public Guid OrderId { get; set; }
    public Order? Order { get; set; }
}