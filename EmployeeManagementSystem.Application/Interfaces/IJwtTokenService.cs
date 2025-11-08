using DiÆon.Attributes;
using EmployeeManagementSystem.Domain.Entities;

namespace EmployeeManagementSystem.Application.Interfaces;

[Scoped]
public interface IJwtTokenService
{
    string GenerateToken(User user);
}