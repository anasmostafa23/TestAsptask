using CafeOrderSystem.Data.Context;
using CafeOrderSystem.Domain.Services;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using CafeOrderSystem.Domain.Interfaces;
using CafeOrderSystem.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<OrderService>();

// Add DbContext and specify the migrations assembly
builder.Services.AddDbContext<CafeDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("CafeOrderSystem.Data")));  // Specify migration assembly

builder.Services.AddScoped<IOrderRepository, OrderRepository>();

// Add controllers with proper JSON settings
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;  // Prevent circular references
        options.JsonSerializerOptions.WriteIndented = true;  // Pretty-print JSON
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure HTTP pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();
app.MapControllers();
app.Run();
