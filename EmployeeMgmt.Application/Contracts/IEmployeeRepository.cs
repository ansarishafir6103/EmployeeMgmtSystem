using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using EmployeeMgmt.Application.DTOs;
using EmployeeMgmt.Domain.Entities;

namespace EmployeeMgmt.Application.Contracts
{
    public interface IEmployeeRepository
    {
        // 1.Create
        Task<bool> AddAsync(Employee employee);
        // 2.Update
        Task<bool> UpdateAsync(Employee employee);
        // 3.Delete
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<Employee>> SearchAsync(string searchTearm);

    }
}
