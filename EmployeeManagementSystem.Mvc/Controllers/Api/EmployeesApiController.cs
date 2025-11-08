using EmployeeManagementSystem.Application.DTOs;
using EmployeeManagementSystem.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EmployeeManagementSystem.Mvc.Controllers.Api;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EmployeesApiController(IEmployeeService employeeService): ControllerBase
{
//------------------------------------------------------------------------------------------------------------------------
    
    [SwaggerOperation(
        Summary = "Retrieve all employees",
        Description = "Returns a list of all employees. Optionally filters results if a search term is provided via query string."
    )]
    [HttpGet(nameof(GetAll))]
    public async Task<IActionResult> GetAll([FromQuery] string? search)
    {
        if (!string.IsNullOrWhiteSpace(search))
        {
            var employees = await employeeService.SearchAsync(search);
            return Ok(employees);
        }

        var allEmployees = await employeeService.GetAllAsync();
        return Ok(allEmployees);
    }
//------------------------------------------------------------------------------------------------------------------------
    
    [SwaggerOperation(
        Summary = "Retrieve employee by ID",
        Description = "Fetches detailed information about a specific employee by their unique ID."
    )]
    [HttpGet("GetById/{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var employee = await employeeService.GetByIdAsync(id);
        if (employee == null)
            return NotFound(new { message = "Employee not found" });
        
        return Ok(employee);
    }
//------------------------------------------------------------------------------------------------------------------------
    
    [SwaggerOperation(
        Summary = "Create a new employee",
        Description = "Adds a new employee record to the system. Requires all mandatory fields in the request body."
    )]
    [HttpPost(nameof(Create))]
    public async Task<IActionResult> Create([FromBody] EmployeeDto employeeDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await employeeService.AddAsync(employeeDto);
        
        if (!result.Success)
            return BadRequest(new { message = result.ErrorMessage });

        return CreatedAtAction(nameof(GetById), new { id = employeeDto.Id }, new { message = "Employee created successfully" });
    }
//------------------------------------------------------------------------------------------------------------------------
    
    [SwaggerOperation(
        Summary = "Update existing employee",
        Description = "Updates employee information by ID. Only valid fields should be provided in the request body."
    )]
    [HttpPut("Update/{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] EmployeeDto employeeDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await employeeService.UpdateAsync(id, employeeDto);
        
        if (!result.Success)
            return BadRequest(new { message = result.ErrorMessage });

        return Ok(new { message = "Employee updated successfully" });
    }
//------------------------------------------------------------------------------------------------------------------------
    
    [SwaggerOperation(
        Summary = "Delete employee",
        Description = "Removes an employee record by their ID. The action is irreversible."
    )]
    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await employeeService.DeleteAsync(id);
        
        if (!result.Success)
            return BadRequest(new { message = result.ErrorMessage });

        return Ok(new { message = "Employee deleted successfully" });
    }
}

