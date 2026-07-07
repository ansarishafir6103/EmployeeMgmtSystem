using EmployeeMgmt.Application.Contracts;
using EmployeeMgmt.Application.DTOs;
using EmployeeMgmt.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeMgmt.Application.Features.Employees.Queries
{
    public class SearchEmployeesQuery : IRequest<IEnumerable<EmployeeResponseDto>>
    {
        public string SearchTearm { get; set; }
        public SearchEmployeesQuery(string searchTerm)
        {
            SearchTearm = searchTerm;
        }
    }
    public class SearchEmployeesQueryHandler : IRequestHandler<SearchEmployeesQuery, IEnumerable<EmployeeResponseDto>>
    {
        private readonly IEmployeeRepository _repository;
        public SearchEmployeesQueryHandler(IEmployeeRepository repository)
        {
            _repository = repository;
        }
        public async Task<IEnumerable<EmployeeResponseDto>> Handle(SearchEmployeesQuery request, CancellationToken cancellationToken)
        {
            // MNC Optimization Rule: Early validation block. Catch empty terms before touching SQL Server.
            if (string.IsNullOrWhiteSpace(request.SearchTearm))
            {
                return new List<EmployeeResponseDto>();
            }

            var employees  = await _repository.SearchAsync(request.SearchTearm.Trim());

            var dtoResult = new List<EmployeeResponseDto>();

            foreach (var emp in employees)
            {
                dtoResult.Add(new EmployeeResponseDto
                {
                    EmployeeID = emp.EmployeeID,
                    FullName = $"{emp.FirstName} {emp.LastName}",
                    Email = emp.Email,
                    Department = emp.Department
                });
            }

            return dtoResult;
        }
    }
}
