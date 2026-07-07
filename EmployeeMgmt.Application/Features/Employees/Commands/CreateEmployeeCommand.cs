using EmployeeMgmt.Application.Contracts;
using EmployeeMgmt.Domain.Entities;
using FluentValidation;
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

        public string Password { get; set; } = string.Empty;
    }
    public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, bool>
    {
        private readonly IEmployeeRepository _repository;
        private readonly IValidator<CreateEmployeeCommand> _validator;
        public CreateEmployeeCommandHandler(IEmployeeRepository repository, IValidator<CreateEmployeeCommand> validator)
        {
            _repository = repository;
            _validator = validator;
        }
        public async Task<bool> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
        {
            // Execute FluentValidation logic
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid) return false;

            string encryptedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var employeeEntity = new Employee
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Department = request.Department,
                HashedPassword = encryptedPassword,
                IsActive = true

            };
            return await _repository.AddAsync(employeeEntity);
        }
    }
}
