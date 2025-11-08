using DiÆon.Attributes;
using EmployeeManagementSystem.Application.DTOs;

namespace EmployeeManagementSystem.Application.Interfaces;

[Scoped]
public interface IAuthService
{
    Task<(bool Success, string? Token, string? ErrorMessage)> RegisterAsync(RegisterDto registerDto);
    Task<(bool Success, string? Token, string? ErrorMessage)> LoginAsync(LoginDto loginDto);
}

