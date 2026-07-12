using EmployeeMgmt.API.Middleware;
using EmployeeMgmt.Application.Contracts;
using EmployeeMgmt.Infrastructure.Persistence;
using EmployeeMgmt.Infrastructure.Repositories;
using EmployeeMgmt.Infrastructure.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Text;
using Microsoft.AspNetCore.OpenApi; 
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// PASTE THIS NEW DBCONTEXT BLOCK EXACTLY HERE
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// विकल्प ए: अगर आप Entity Framework Core चलाना चाहते हैं, तो इसे अनकमेंट रखें:
builder.Services.AddScoped<IEmployeeRepository, EfEmployeeRepository>();

// ⭐ REGISTER YOUR CUSTOM JWT TOKEN SERVICE CORE AS SCRAMBLED SCOPED DEPENDENCY ⭐
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

// ⭐ CONFIGURE MICROSOFT AUTHENTICATION SERVICES MIDDLEWARE PARAMETERS ⭐
var secretKeyBytes = Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]!);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),
        ClockSkew = TimeSpan.Zero // Forces immediate token expiration times
    };
});


//// Registering our Dependency Injection rule (MNC Standard)
//builder.Services.AddScoped<IEmployeeRepository, AdoEmployeeRepository>();

// Registering MediatR to route all our Commands and Queries automatically
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(IEmployeeRepository).Assembly));

// Automatically registers all AbstractValidator classes found inside your Application layer
builder.Services.AddValidatorsFromAssembly(typeof(IEmployeeRepository).Assembly);

// Core services required for Swagger documentation tool
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
// =========================================================================
// 1. SWAGGER JWT SECURITY CONFIGURATION (100% ERROR-FREE SYNTX)
// =========================================================================
builder.Services.AddSwaggerGen(options =>
{
    // v1 डॉक्यूमेंटेशन सेट करना (बिना किसी नेमस्पेस एरर के)
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Employee Management API",
        Version = "v1"
    });

    // 1. JWT Bearer सुरक्षा की परिभाषा बताना
    var securityScheme = new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Type 'Bearer' followed by a space and then paste your token.\n\nExample: Bearer eyJhbGciOi...",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Reference = new Microsoft.OpenApi.Models.OpenApiReference
        {
            Id = "Bearer",
            Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme
        }
    };

    options.AddSecurityDefinition("Bearer", securityScheme);

    // 2. सुरक्षा आवश्यकताओं (Security Requirement) को लागू करना ताकि ताला दिख सके
    var securityRequirement = new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        { securityScheme, Array.Empty<string>() }
    };

    options.AddSecurityRequirement(securityRequirement);
});


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

app.UseAuthentication();
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
