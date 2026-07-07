using EmployeeMgmt.Application.Features.Employees.Commands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeMgmt.Application.Features.Employees.Validators
{
    public class CreateEmployeeCommandValidator:AbstractValidator<CreateEmployeeCommand>
    {
        public CreateEmployeeCommandValidator()
        {
            RuleFor(p=>p.FirstName)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.")
                .Matches(@"^[a-zA-Z\s]*$").WithMessage("{PropertyName} can only contain alphabetic letters.");

            RuleFor(p => p.LastName)
                .NotEmpty().WithMessage("{PropertyName} is required.");

            RuleFor(p => p.Email)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .EmailAddress().WithMessage("A valid email address is required.");

            RuleFor(p => p.Department)
                .NotEmpty().WithMessage("{PropertyName} is required.");

            RuleFor(p => p.Password)
               .NotEmpty().WithMessage("{PropertyName} is required.")
               .MinimumLength(8).WithMessage("{PropertyName} must be at least 8 characters long.");
        }
    }
}
