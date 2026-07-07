using EmployeeMgmt.Application.Contracts;
using EmployeeMgmt.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Registering our Dependency Injection rule (MNC Standard)
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();

// Registering MediatR to route all our Commands and Queries automatically
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(IEmployeeRepository).Assembly));

// Core services required for Swagger documentation tool
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// =========================================================================
// 2. HTTP REQUEST PIPELINE (Configure how requests flow through our app)
// =========================================================================

// ACTIVATING SWAGGER: These two lines must be here to fix the 404 page error!
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Employee Management API V1");
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
