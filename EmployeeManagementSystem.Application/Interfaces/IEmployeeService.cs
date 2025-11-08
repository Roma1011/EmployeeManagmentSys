using DiÆon.Attributes;
using EmployeeManagementSystem.Application.DTOs;

namespace EmployeeManagementSystem.Application.Interfaces;

[Scoped]
public interface IEmployeeService
{
    Task<IEnumerable<EmployeeDto>> GetAllAsync();
    Task<IEnumerable<EmployeeDto>> SearchAsync(string searchTerm);
    Task<EmployeeDto?> GetByIdAsync(int id);
    Task<(bool Success, string? ErrorMessage)> AddAsync(EmployeeDto employeeDto);
    Task<(bool Success, string? ErrorMessage)> UpdateAsync(int id, EmployeeDto employeeDto);
    Task<(bool Success, string? ErrorMessage)> DeleteAsync(int id);
}

