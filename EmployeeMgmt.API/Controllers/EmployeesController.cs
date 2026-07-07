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
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateEmployeeCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result)
            {
                return BadRequest(new { Message = "Failed to add employee. Please check the provided data." });
            }

            return StatusCode(StatusCodes.Status201Created, new { Message = "Employee successfully added to the database." });
        }
        [HttpGet("search")]
        [ProducesResponseType(typeof(IEnumerable<EmployeeResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Search([FromQuery] string? term = "")
        {
           // throw new Exception("SQL Server database connection timed out unexpectedly!");

            var result = await _mediator.Send(new SearchEmployeesQuery(term ?? ""));
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
                return BadRequest(new { Message = "Failed to update employee details. Record may not exist." });
            }
            return Ok(new { Message = "Employee details successfully updated in the database." });
        }
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteEmployeeCommand(id));

            if (!result)
            {
                return BadRequest(new { Message = "Failed to remove employee record. The ID may be invalid or non-existent." });
            }
            return Ok(new { Message = "Employee successfully removed from the system (Soft Delete)." });
        }


        // =========================================================================
        // 5. CHANGE PASSWORD (POST) - Dedicated secure mutation route
        // =========================================================================
        [HttpPost("change-password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result)
            {
                return BadRequest(new { Message = "Failed to update password. Verify your current credentials or ID." });
            }
            return Ok(new { Message = "Your system password has been successfully updated." });
        }

    }
}
