using EmployeeMgmt.Application.Contracts;
using EmployeeMgmt.Domain.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace EmployeeMgmt.Infrastructure.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly string _connectionString;

        public EmployeeRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                                 ?? throw new ArgumentNullException("Connection string not found.");
        }
        public async Task<bool> AddAsync(Employee employee)
        {
            string query = @"INSERT INTO tblEmployees (FirstName, LastName, Email, Department) 
                             VALUES (@FirstName, @LastName, @Email, @Department)";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@FirstName", employee.FirstName);
            cmd.Parameters.AddWithValue("@LastName", employee.LastName);
            cmd.Parameters.AddWithValue("@Email", employee.Email);
            cmd.Parameters.AddWithValue("@Department", employee.Department);

            await conn.OpenAsync();
            int rowsAffected = await cmd.ExecuteNonQueryAsync();
            return rowsAffected > 0;

        }
        public async Task<IEnumerable<Employee>> SearchAsync(string searchTearm)
        {
            var employees = new List<Employee>();

            string query = @"SELECT * FROM tblEmployees 
                            WHERE IsActive = 1 AND 
                            (FirstName like @Search OR LastName like @Search OR Department like @Search)";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(@query, conn);
                
            cmd.Parameters.AddWithValue("@Search", $"%{searchTearm}%");

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                employees.Add(new Employee
                {
                    EmployeeID = (int)reader["EmployeeID"],
                    FirstName = reader["FirstName"].ToString() ?? string.Empty,
                    LastName = reader["LastName"].ToString() ?? string.Empty,
                    Email = reader["Email"].ToString() ?? string.Empty,
                    Department = reader["Department"].ToString() ?? string.Empty,
                    IsActive = (bool)reader["IsActive"],
                    CreatedDate = (DateTime)reader["CreatedDate"]
                });
            }
            return employees;
        }
        public async Task<bool> UpdateAsync(Employee employee)
        {
            string query = @"UPDATE tblEmployees
                             SET FirstName=@FirstName,LastName = @LastName,Department=@Department
                             WHERE EmployeeID =@EmployeeID AND IsActive = 1";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@EmployeeID",employee.EmployeeID);
            cmd.Parameters.AddWithValue("@FirstName",employee.FirstName);
            cmd.Parameters.AddWithValue("@LastName",employee.LastName);
            cmd.Parameters.AddWithValue("@Department",employee.Department);

            await conn.OpenAsync();
            int rowsAffected = await cmd.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            // Soft Delete Query: Flips the IsActive bit to 0 instead of dropping the row
            string query = "UPDATE tblEmployees SET IsActive = 0 WHERE EmployeeID = @EmployeeID";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@EmployeeID", id);

            await conn.OpenAsync();
            int rowsAffected = await cmd.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }

    }
}
