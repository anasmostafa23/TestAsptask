namespace CafeOrderSystem.Domain.Entities;
using System.Text.Json.Serialization;

public class OrderItem
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string ItemName { get; set; }
    public Guid OrderId { get; set; }

    [JsonIgnore]
    public Order? Order { get; set; }
}