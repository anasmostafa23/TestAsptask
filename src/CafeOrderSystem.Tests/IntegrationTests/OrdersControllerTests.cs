using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CafeOrderSystem.API;
using CafeOrderSystem.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.TestHost;

public class OrdersControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public OrdersControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetOrders_ShouldReturnOk()
    {
        var response = await _client.GetAsync("/orders");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task CreateOrder_ShouldReturnCreated()
    {
        var order = new Order
        {
            CustomerName = "Jane Doe",
            PaymentMethod = "PayPal",
            OrderItems = new List<OrderItem>
            {
                new OrderItem { ItemName = "Espresso" }
            }
        };

        var response = await _client.PostAsJsonAsync("/orders", order);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }
}
