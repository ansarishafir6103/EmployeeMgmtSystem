using EmployeeMgmt.Application.Contracts;
using EmployeeMgmt.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeMgmt.Application.Features.Employees.Queries
{
    public class SearchEmployeesQuery:IRequest<IEnumerable<Employee>>
    {
        public string SearchTearm { get; set; }
        public SearchEmployeesQuery(string searchTerm)
        {
            SearchTearm = searchTerm;
        }
    }
    public class SearchEmployeesQueryHandler:IRequestHandler<SearchEmployeesQuery,IEnumerable<Employee>>
    {
        private readonly IEmployeeRepository _repository;
        public SearchEmployeesQueryHandler(IEmployeeRepository repository)
        {
            _repository = repository;
        }
        public async Task<IEnumerable<Employee>> Handle(SearchEmployeesQuery request,CancellationToken cancellationToken)
        {
            if(string.IsNullOrWhiteSpace(request.SearchTearm))
            {
                return new List<Employee>();
            }
            return await _repository.SearchAsync(request.SearchTearm.Trim());
        }
}
}
