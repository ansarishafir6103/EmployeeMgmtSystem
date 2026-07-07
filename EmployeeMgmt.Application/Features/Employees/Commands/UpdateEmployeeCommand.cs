using EmployeeMgmt.Application.Contracts;
using EmployeeMgmt.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace EmployeeMgmt.Application.Features.Employees.Commands
{
    public class UpdateEmployeeCommand : IRequest<bool>
    {
        public int EmployeeID { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
    }
    public class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployeeCommand, bool>
    {
        private readonly IEmployeeRepository _repository;
        public UpdateEmployeeCommandHandler(IEmployeeRepository repository)
        {
            _repository = repository;
        }
        public async Task<bool> Handle(UpdateEmployeeCommand request,CancellationToken cancellationToken)
        {
            if(request.EmployeeID == 0 || string.IsNullOrWhiteSpace(request.FirstName)) 
            {
                return false;
            }
            var updateEmployee = new Employee
            {
                EmployeeID = request.EmployeeID,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Department = request.Department
            };
            return await _repository.UpdateAsync(updateEmployee);
        }
    }
}