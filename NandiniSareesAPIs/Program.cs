using Microsoft.EntityFrameworkCore;
using NandiniSareesAPIs.Models;
using NandiniSareesAPIs.Features.Products;
using NandiniSareesAPIs.Features.Users;
using MediatR;
using NandiniSareesAPIs.Features.ProductImages.Commands;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// CORS: allow requests from any origin (adjust for production)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Register DbContext for EF Core (reads connection string "DefaultConnection" from appsettings.json)
builder.Services.AddDbContext<NandiniSareesDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions
            .EnableRetryOnFailure(
                maxRetryCount: 2,
                maxRetryDelay: System.TimeSpan.FromSeconds(30),
                errorNumbersToAdd: null)
            .CommandTimeout(60)
    ));

// Register CQRS-friendly interfaces
builder.Services.AddScoped<IReadDbContext>(sp => sp.GetRequiredService<NandiniSareesDbContext>());
builder.Services.AddScoped<IWriteDbContext>(sp => sp.GetRequiredService<NandiniSareesDbContext>());
// Register product CQRS services
builder.Services.AddScoped<IProductQueries, ProductQueries>();
builder.Services.AddScoped<IProductCommands, ProductCommands>();
builder.Services.AddScoped<IUserQueries, UserQueries>();
builder.Services.AddScoped<IUserCommands, UserCommands>();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// 1. Learn more about configuring Swagger/OpenAPI at https://aka.ms
builder.Services.AddEndpointsApiExplorer(); // Required for minimal APIs / routing
builder.Services.AddSwaggerGen();           // Registers the Swagger generator
// Register controller services so MapControllers() can find required MVC services
builder.Services.AddControllers();

// Register MediatR handlers
builder.Services.AddMediatR(typeof(UploadProductImageCommand).Assembly);

// Register authentication and authorization services.
// Adjust the authentication scheme and options (e.g., JWT bearer) as needed for your app.
//builder.Services.AddAuthentication(Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)
//       .AddJwtBearer(options =>
//       {
//           // TODO: Configure JWT options: Authority, Audience, TokenValidationParameters, etc.
//       });

// Ensure authentication middleware runs before authorization
builder.Services.AddAuthorization();
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
// Enable Swagger/OpenAPI in all environments so the deployed site can access the UI.
app.MapOpenApi();
app.UseSwagger();   // Serves the OpenAPI/Swagger JSON document
app.UseSwaggerUI(); // Serves the interactive Swagger UI web page

app.UseHttpsRedirection();

// Serve static files (uploaded images)
app.UseStaticFiles();

// Use CORS policy
app.UseCors("AllowAll");

app.MapControllers();

// Map root to Swagger UI so visitors land on API documentation.
app.MapGet("/", () => Results.Redirect("/swagger/index.html"));
///weatherforecast

app.Run();
