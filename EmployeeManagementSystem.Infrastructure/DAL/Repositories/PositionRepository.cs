using DiÆon.Attributes;
using EmployeeManagementSystem.Domain.Entities;
using EmployeeManagementSystem.Domain.Interfaces;
using EmployeeManagementSystem.Infrastructure.DAL;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Infrastructure.DAL.Repositories;

[Scoped]
internal class PositionRepository(ApplicationDbContext context): IPositionRepository
{
    public async Task<Position?> GetByIdAsync(int id)
        => await context.Positions
            .Include(p => p.Parent)
            .Include(p => p.Children)
            .FirstOrDefaultAsync(p => p.Id == id);

    public async Task<IEnumerable<Position>> GetAllAsync()
        => await context.Positions
            .Include(p => p.Parent)
            .Include(p => p.Children)
            .ToListAsync();

    public async Task<IEnumerable<Position>> GetTreeAsync()
        => await context.Positions
            .Include(p => p.Parent)
            .Include(p => p.Children)
            .ToListAsync();

    public async Task<Position> AddAsync(Position position)
    {
        context.Positions.Add(position);
        await context.SaveChangesAsync();
        return position;
    }

    public async Task DeleteAsync(int id)
    {
        var position = await context.Positions.FindAsync(id);
        if (position != null)
        {
            context.Positions.Remove(position);
            await context.SaveChangesAsync();
        }
    }

    public async Task<bool> HasChildrenAsync(int id)
        => await context.Positions.AnyAsync(p => p.ParentId == id);
}

