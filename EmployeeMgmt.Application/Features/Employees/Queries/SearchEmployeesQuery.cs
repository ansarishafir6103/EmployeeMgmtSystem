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
        public string SearchTerm { get; set; }
        public SearchEmployeesQuery(string searchTerm)
        {
            SearchTerm = searchTerm;
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
            string safeSearchTerm = string.IsNullOrWhiteSpace(request.SearchTerm) ? "" : request.SearchTerm.Trim().Replace("'","");

            var employees  = await _repository.SearchAsync(safeSearchTerm);

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
