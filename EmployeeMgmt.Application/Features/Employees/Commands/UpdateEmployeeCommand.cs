using EmployeeMgmt.Application.Contracts;
using EmployeeMgmt.Domain.Entities;
using FluentValidation;
using MediatR;

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
        private readonly IValidator<UpdateEmployeeCommand> _validator;
        public UpdateEmployeeCommandHandler(IEmployeeRepository repository, IValidator<UpdateEmployeeCommand> validator)
        {
            _repository = repository;
            _validator = validator;
        }
        public async Task<bool> Handle(UpdateEmployeeCommand request,CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            // If validation rules break, exit early and return false
            if (!validationResult.IsValid)
            {
                return false;
            }

            var updatedEmployee = new Employee
            {
                EmployeeID = request.EmployeeID,
                FirstName = request.FirstName.Trim(),
                LastName = request.LastName.Trim(),
                Department = request.Department.Trim()
            };

            return await _repository.UpdateAsync(updatedEmployee);
        }
    }
}