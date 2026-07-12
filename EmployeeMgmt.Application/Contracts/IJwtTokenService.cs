using EmployeeMgmt.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeMgmt.Application.Contracts
{
    public interface IJwtTokenService
    {
        string GenerateToken(Employee employee, string role);
    }
}
