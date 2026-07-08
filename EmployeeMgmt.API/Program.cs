using EmployeeMgmt.API.Middleware;
using EmployeeMgmt.Application.Contracts;
using EmployeeMgmt.Infrastructure.Persistence;
using EmployeeMgmt.Infrastructure.Repositories;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// PASTE THIS NEW DBCONTEXT BLOCK EXACTLY HERE
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// विकल्प ए: अगर आप Entity Framework Core चलाना चाहते हैं, तो इसे अनकमेंट रखें:
builder.Services.AddScoped<IEmployeeRepository, EfEmployeeRepository>();

//// Registering our Dependency Injection rule (MNC Standard)
//builder.Services.AddScoped<IEmployeeRepository, AdoEmployeeRepository>();

// Registering MediatR to route all our Commands and Queries automatically
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(IEmployeeRepository).Assembly));

// Automatically registers all AbstractValidator classes found inside your Application layer
builder.Services.AddValidatorsFromAssembly(typeof(IEmployeeRepository).Assembly);

// Core services required for Swagger documentation tool
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//  PASTE THIS EXCEPTION MIDDLEWARE SHIELD EXACTLY HERE 
app.UseMiddleware<ExceptionMiddleware>();

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

// =========================================================================
// MNC DEVELOPMENT BEST PRACTICE: AUTOMATIC DATABASE MIGRATION ON STARTUP
// =========================================================================
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        await context.Database.MigrateAsync();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while auto-applying database migrations.");
    }
}
app.Run();
