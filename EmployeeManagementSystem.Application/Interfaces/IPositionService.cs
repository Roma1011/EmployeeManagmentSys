using DiÆon.Attributes;
using EmployeeManagementSystem.Application.DTOs;

namespace EmployeeManagementSystem.Application.Interfaces;

[Scoped]
public interface IPositionService
{
    Task<IEnumerable<PositionDto>> GetTreeAsync();
    Task<(bool Success, string? ErrorMessage)> AddAsync(PositionDto positionDto);
    Task<(bool Success, string? ErrorMessage)> DeleteAsync(int id);
}

