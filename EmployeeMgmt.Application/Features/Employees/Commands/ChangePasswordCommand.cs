using EmployeeMgmt.Application.Contracts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeMgmt.Application.Features.Employees.Commands
{
    // 1. The Command Payload (Input Data)
    public class ChangePasswordCommand : IRequest<bool>
    {
        public int EmployeeID { get; set; }
        public string OldPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, bool>
    {
        private readonly IEmployeeRepository _repository;

        public ChangePasswordCommandHandler(IEmployeeRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            // Structural Check
            if (request.EmployeeID <= 0 || string.IsNullOrWhiteSpace(request.OldPassword) || string.IsNullOrWhiteSpace(request.NewPassword))
            {
                return false;
            }

            // 1. Fetch the exact employee record along with their current hash
            var employee = await _repository.GetByIdWithPasswordAsync(request.EmployeeID);
            if (employee == null || !employee.IsActive)
            {
                return false; // Employee record not found or deactivated
            }

            // 2. Verify if the provided OldPassword matches the hashed string in the database
            bool isOldPasswordCorrect = BCrypt.Net.BCrypt.Verify(request.OldPassword, employee.HashedPassword);
            if (!isOldPasswordCorrect)
            {
                return false; // Authentication failed
            }

            // 3. Cryptographically hash the new text password string cleanly
            string newHashedPassword = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);

            // 4. Fire the dedicated update function in the data layer
            return await _repository.UpdatePasswordAsync(request.EmployeeID, newHashedPassword);
        }
    }
}
