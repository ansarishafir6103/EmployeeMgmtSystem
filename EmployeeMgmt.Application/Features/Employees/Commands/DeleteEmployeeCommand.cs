using EmployeeMgmt.Application.Contracts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeeMgmt.Application.Features.Employees.Commands
{
    public class DeleteEmployeeCommand:IRequest<bool>
    {
        public int EmployeeID { get; set; }
        public DeleteEmployeeCommand(int employeeId)
        {
            EmployeeID = employeeId;
        }
    }
    public class DeleteEmployeeCommandHandler :IRequestHandler<DeleteEmployeeCommand,bool>
    {
        private readonly IEmployeeRepository _repository;
        public DeleteEmployeeCommandHandler(IEmployeeRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
        {
            if(request.EmployeeID <= 0)
            {
                return false;
            }
            return await _repository.DeleteAsync(request.EmployeeID);
        }
    }
}
