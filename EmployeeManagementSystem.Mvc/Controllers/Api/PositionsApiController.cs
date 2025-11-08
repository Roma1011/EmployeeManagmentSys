using EmployeeManagementSystem.Application.DTOs;
using EmployeeManagementSystem.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EmployeeManagementSystem.Mvc.Controllers.Api;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PositionsApiController(IPositionService positionService): ControllerBase
{
 //------------------------------------------------------------------------------------------------------------------------

    [SwaggerOperation(
        Summary = "Retrieve hierarchical position tree",
        Description = "Returns all positions structured as a hierarchical tree, where each position may have nested child positions."
    )]
    [HttpGet(nameof(GetTree))]
    public async Task<IActionResult> GetTree()
    {
        var positions = await positionService.GetTreeAsync();
        return Ok(positions);
    }
 //------------------------------------------------------------------------------------------------------------------------
    
    [SwaggerOperation(
        Summary = "Create a new position",
        Description = "Adds a new position to the system. Optionally, a parent position can be specified using the ParentId field."
    )]
    [HttpPost(nameof(Create))]
    public async Task<IActionResult> Create([FromBody] PositionDto positionDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await positionService.AddAsync(positionDto);
        
        if (!result.Success)
            return BadRequest(new { message = result.ErrorMessage });

        return Ok(new { message = "Position created successfully" });
    }
 //------------------------------------------------------------------------------------------------------------------------
    
    [SwaggerOperation(
        Summary = "Delete position",
        Description = "Deletes a position by its ID. If the position has child positions, deletion may be restricted or require cascading removal depending on business rules."
    )]
    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await positionService.DeleteAsync(id);
        
        if (!result.Success)
            return BadRequest(new { message = result.ErrorMessage });

        return Ok(new { message = "Position deleted successfully" });
    }
}

