using EmployeeManagementSystem.Application.DTOs;
using EmployeeManagementSystem.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EmployeeManagementSystem.Mvc.Controllers.Api;

[ApiController]
[Route("api/[controller]")]
public class AuthApiController(IAuthService authService): ControllerBase
{
//------------------------------------------------------------------------------------------------------------------------
    [SwaggerOperation(
        Summary = nameof(Register),
        Description = "Registers a new user. Validates personal number (must be 11 digits), email format, required fields, " +
                      "and checks whether a user with the same email already exists. " +
                      "If registration succeeds, returns a JWT token and success message."
    )]
    [HttpPost(nameof(Register))]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var result = await authService.RegisterAsync(registerDto);
    
        if (!result.Success)
            return BadRequest(new { message = result.ErrorMessage });

        return Ok(new { token = result.Token, message = "Registration successful" });
    }
//------------------------------------------------------------------------------------------------------------------------
    [SwaggerOperation(
        Summary = nameof(Login),
        Description = "Authenticates a registered user. Accepts username (email) and password. " +
                      "If credentials are valid, returns a JWT token and success message; " +
                      "otherwise, returns 401 Unauthorized with an error message."
    )]
    [HttpPost(nameof(Login))]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await authService.LoginAsync(loginDto);
    
        if (!result.Success)
            return Unauthorized(new { message = result.ErrorMessage });

        return Ok(new { token = result.Token, message = "Login successful" });
    }
}

