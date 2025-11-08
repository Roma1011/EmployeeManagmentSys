using DiÆon.Attributes;
using EmployeeManagementSystem.Domain.Entities;

namespace EmployeeManagementSystem.Domain.Interfaces;

[Scoped]
public interface IPositionRepository
{
    Task<Position?> GetByIdAsync(int id);
    Task<IEnumerable<Position>> GetAllAsync();
    Task<IEnumerable<Position>> GetTreeAsync();
    Task<Position> AddAsync(Position position);
    Task DeleteAsync(int id);
    Task<bool> HasChildrenAsync(int id);
}

