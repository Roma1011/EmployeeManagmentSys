using DiÆon.Attributes;
using EmployeeManagementSystem.Domain.Entities;

namespace EmployeeManagementSystem.Domain.Interfaces;

[Scoped]
public interface IEmployeeRepository
{
    Task<Employee?> GetByIdAsync(int id);
    Task<IEnumerable<Employee>> GetAllAsync();
    Task<IEnumerable<Employee>> SearchAsync(string searchTerm);
    Task<Employee> AddAsync(Employee employee);
    Task<Employee> UpdateAsync(Employee employee);
    Task DeleteAsync(int id);
    Task<bool> ExistsByPersonalNumberAsync(string personalNumber);
    Task<bool> ExistsByEmailAsync(string email);
}

