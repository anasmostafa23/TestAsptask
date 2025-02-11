namespace CafeOrderSystem.Domain.Entities;
public class Order
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string CustomerName { get; set; }
    public DateTime OrderTime { get; set; } = DateTime.UtcNow;
    public decimal TotalAmount { get; set; }
    public required string PaymentMethod { get; set; }
    public string Status { get; set; } = "InProgress";
    public List<OrderItem> OrderItems { get; set; } = new();
}