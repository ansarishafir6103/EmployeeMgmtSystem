using EmployeeMgmt.Application.Contracts;
using EmployeeMgmt.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeMgmt.Application.Features.Employees.Commands
{
    public class CreateEmployeeCommand : IRequest<bool>
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
    }
    public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, bool>
    {
        private IEmployeeRepository _repository;
        public CreateEmployeeCommandHandler(IEmployeeRepository repository)
        {
            _repository = repository;
        }
        public async Task<bool> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.FirstName))
            {
                return false;
            }
            var employeeEntity = new Employee
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Department = request.Department
            };
            return await _repository.AddAsync(employeeEntity);
        }
    }
}
