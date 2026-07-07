using EmployeeMgmt.Application.DTOs;
using EmployeeMgmt.Application.Features.Employees.Commands;
using EmployeeMgmt.Application.Features.Employees.Queries;
using EmployeeMgmt.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeMgmt.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : ControllerBase
    {

        private readonly IMediator _mediator;

        public EmployeesController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateEmployeeCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result)
            {
                return BadRequest("Failed to add employee. Please check the provided data.");
            }
            return Ok("Employee successfully added to the database.");
        }
        [HttpGet("search")]
        [ProducesResponseType(typeof(IEnumerable<EmployeeResponseDto>),StatusCodes.Status200OK)]
        public async Task<IActionResult> Search([FromQuery] string term)
        {
            var result = await _mediator.Send(new SearchEmployeesQuery(term));
            return Ok(result);
        }
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromBody] UpdateEmployeeCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result)
            {
                return BadRequest("Failed to update employee details. Record may not exist.");
            }
            return Ok("Employee details successfully updated in the database.");
        }
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteEmployeeCommand(id));

            if (!result)
            {
                return BadRequest("Failed to remove employee record. The ID may be invalid or non-existent.");
            }
            return Ok("Employee successfully removed from the system (Soft Delete).");
        }
    }
}
