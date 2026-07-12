using EmployeeMgmt.Application.Contracts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeMgmt.Application.Features.Employees.Commands
{
    // The Input Login Payload Request mapping
    public class LoginEmployeeCommand : IRequest<LoginResponseDto?>
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class LoginResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
    // Login Authentication processing handler engine
    public class LoginEmployeeCommandHandler : IRequestHandler<LoginEmployeeCommand, LoginResponseDto?>
    {
        private readonly IEmployeeRepository _repository;
        private readonly IJwtTokenService _tokenService;

        public LoginEmployeeCommandHandler(IEmployeeRepository repository, IJwtTokenService tokenService)
        {
            _repository = repository;
            _tokenService = tokenService;
        }

        public async Task<LoginResponseDto?> Handle(LoginEmployeeCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
                return null;

            // 1. Fetch data from underlying tracking layer
            var employee = await _repository.GetByEmailAsync(request.Email.Trim().ToLower());
            if (employee == null) return null; // Email validation mismatch check

            // 2. Verify hashed cryptographic strings matches safely using BCrypt verify
            bool isPasswordCorrect = BCrypt.Net.BCrypt.Verify(request.Password, employee.HashedPassword);
            if (!isPasswordCorrect) return null; // Password failure mismatch check

            // 3. Determine structural access role (MNC example: hardcoding for simplicity)
            string assignedRole = request.Email.Contains("admin") ? "Admin" : "Employee";

            // 4. Fire service to sign token payload 
            string JWTstring = _tokenService.GenerateToken(employee, assignedRole);

            return new LoginResponseDto { Token = JWTstring, Email = employee.Email };
        }
    }
}
