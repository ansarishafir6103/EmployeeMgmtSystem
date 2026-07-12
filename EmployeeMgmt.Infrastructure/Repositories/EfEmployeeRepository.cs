using EmployeeMgmt.Application.Contracts;
using EmployeeMgmt.Domain.Entities;
using EmployeeMgmt.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeMgmt.Infrastructure.Repositories
{
    public class EfEmployeeRepository : IEmployeeRepository
    {

        private readonly ApplicationDbContext _context;

        public EfEmployeeRepository(ApplicationDbContext context) => _context = context;


        public async Task<bool> AddAsync(Employee employee)
        {
            await _context.Employees.AddAsync(employee);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null) return false;
            employee.IsActive = false;
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<Employee>> SearchAsync(string searchTerm)
        {
            var query =  _context.Employees.Where(e => e.IsActive);
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(e => e.FirstName.Contains(searchTerm) ||
                                          e.LastName.Contains(searchTerm) ||
                                          e.Department.Contains(searchTerm));
            }
            return await query.ToListAsync();
        }

        public async Task<bool> UpdateAsync(Employee employee)
        {
            var existing = await _context.Employees.FirstOrDefaultAsync(e => e.EmployeeID == employee.EmployeeID && e.IsActive);
            if (existing == null) return false;

            existing.FirstName = employee.FirstName;
            existing.LastName = employee.LastName;
            existing.Email = employee.Email;
            existing.Department = employee.Department;

            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<Employee?> GetByIdWithPasswordAsync(int id)
        {
            // EF Core tracks the row mapping based on the implicit primary key
            return await _context.Employees.FirstOrDefaultAsync(e => e.EmployeeID == id && e.IsActive);
        }

        public async Task<bool> UpdatePasswordAsync(int id, string newHashedPassword)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null) return false;

            // Mutate only the targeted security field
            employee.HashedPassword = newHashedPassword;

            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<Employee?> GetByEmailAsync(string email) =>
        await _context.Employees.FirstOrDefaultAsync(e => e.Email == email && e.IsActive);

    }
}
