using FluentValidation;
using EmployeeMgmt.Application.Features.Employees.Commands;

namespace EmployeeMgmt.Application.Features.Employees.Validators
{
    // MNC Standard: Validation rules isolated from the data carrier class
    public class UpdateEmployeeCommandValidator : AbstractValidator<UpdateEmployeeCommand>
    {
        public UpdateEmployeeCommandValidator()
        {
            // Crucial check for Updates: Employee ID must be provided and valid
            RuleFor(p => p.EmployeeID)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .GreaterThan(0).WithMessage("{PropertyName} must be a valid positive identifier.");

            RuleFor(p => p.FirstName)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.")
                .Matches(@"^[a-zA-Z\s]*$").WithMessage("{PropertyName} can only contain alphabetic letters.");

            RuleFor(p => p.LastName)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.")
                .Matches(@"^[a-zA-Z\s]*$").WithMessage("{PropertyName} can only contain alphabetic letters.");

            RuleFor(p => p.Department)
                .NotEmpty().WithMessage("{PropertyName} is required.");
        }
    }
}
