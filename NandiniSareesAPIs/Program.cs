using Microsoft.EntityFrameworkCore;
using NandiniSareesAPIs.Models;
using NandiniSareesAPIs.Features.Products;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Register DbContext for EF Core (reads connection string "DefaultConnection" from appsettings.json)
builder.Services.AddDbContext<NandiniSareesDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register CQRS-friendly interfaces
builder.Services.AddScoped<IReadDbContext>(sp => sp.GetRequiredService<NandiniSareesDbContext>());
builder.Services.AddScoped<IWriteDbContext>(sp => sp.GetRequiredService<NandiniSareesDbContext>());
// Register product CQRS services
builder.Services.AddScoped<IProductQueries, ProductQueries>();
builder.Services.AddScoped<IProductCommands, ProductCommands>();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// 1. Learn more about configuring Swagger/OpenAPI at https://aka.ms
builder.Services.AddEndpointsApiExplorer(); // Required for minimal APIs / routing
builder.Services.AddSwaggerGen();           // Registers the Swagger generator

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();   // Serves the OpenAPI/Swagger JSON document
    app.UseSwaggerUI(); // Serves the interactive Swagger UI web page
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Map root to an existing endpoint so requests to '/' don't return 404.
app.MapGet("/", () => Results.Redirect("/api"));
///weatherforecast

app.Run();
