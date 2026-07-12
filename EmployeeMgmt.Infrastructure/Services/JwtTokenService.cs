using EmployeeMgmt.Application.Contracts;
using EmployeeMgmt.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EmployeeMgmt.Infrastructure.Services
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly IConfiguration _config;

        public JwtTokenService(IConfiguration config) => _config = config;

        public string GenerateToken(Employee employee, string role)
        {
            // 1. CLAIMS ASSEMBLING: Packing user data securely into the token payload
            var claims = new List<Claim>
            {
                new Claim (JwtRegisteredClaimNames.Sub,employee.EmployeeID.ToString()),
                new Claim (JwtRegisteredClaimNames.Email,employee.Email),
                new Claim (ClaimTypes.Name,$"{employee.FirstName} {employee.LastName}"),
                new Claim (ClaimTypes.Role,role)
            };
            // 2. CRYPTOGRAPHIC SIGNING: Binding the signature using our Secret Key
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:SecretKey"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // 3. TOKEN CONSTRUCTION
            var token = new JwtSecurityToken(
                issuer: _config["JwtSettings:Issuer"],
                audience: _config["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_config["JwtSettings:ExpiryInMinutes"])),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
