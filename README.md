# EmployeeMgmtSystem
CREATE CRUD API FOR MNC COMPANY ARICHITACTURE

# Employee Management System API

This is an enterprise-grade backend system built using **ASP.NET Core**, adhering to **MNC-standard Clean Architecture (Onion Architecture)**, **CQRS Pattern** via MediatR, and **Dependency Injection** principles. It seamlessly supports dual database execution engines utilizing both **ADO.NET** and **Entity Framework Core**.

---

## 🛠️ Database Setup and EF Core Migrations

Before launching the API application for the first time, you must apply the database migrations to auto-generate the SQL Server table schemas.

### Prerequisites
1. Ensure your connection string details (Server, User ID, and Password) are correctly configured inside the `4_Presentation/EmployeeMgmt.API/appsettings.json` file.
2. Open the **Package Manager Console** inside Visual Studio (`Tools > NuGet Package Manager > Package Manager Console`).

### Step 1: Create a New Migration
Generate a fresh migration schema by executing this command inside the console (ensure the Default Project dropdown is set to `EmployeeMgmt.Infrastructure`):

\`\`\`powershell
Add-Migration InitialCreate --project EmployeeMgmt.Infrastructure --startup-project EmployeeMgmt.API
\`\`\`

### Step 2: Apply Schema to SQL Server
Physically apply the structural migrations and update your local database instances by executing:

\`\`\`powershell
Update-Database --project EmployeeMgmt.Infrastructure --startup-project EmployeeMgmt.API
\`\`\`

---

## 🎛️ Swapping Data Access Engines (ADO.NET vs EF Core)

Thanks to the interface decoupling and Dependency Injection setup, you can turn off the EF Core engine and switch back to native raw ADO.NET at any time with a single modification.

Open `Program.cs` inside the API project and swap the scoped implementations:

\`\`\`csharp
// Option A: Active Entity Framework Core Engine (Default)
builder.Services.AddScoped<IEmployeeRepository, EfEmployeeRepository>();

// Option B: Active Native ADO.NET Engine
// builder.Services.AddScoped<IEmployeeRepository, AdoEmployeeRepository>();
\`\`\`
