using DiÆon.Attributes;
using EmployeeManagementSystem.Domain.Entities;
using EmployeeManagementSystem.Domain.Interfaces;
using EmployeeManagementSystem.Infrastructure.DAL;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Infrastructure.DAL.Repositories;

[Scoped]
internal class EmployeeRepository(ApplicationDbContext ctx): IEmployeeRepository
{
    public async Task<Employee?> GetByIdAsync(int id)
        => await ctx.Employees
            .Include(e => e.Position)
            .FirstOrDefaultAsync(e => e.Id == id);

    public async Task<IEnumerable<Employee>> GetAllAsync()
        => await ctx.Employees
            .Include(e => e.Position)
            .OrderBy(e => e.LastName)
            .ThenBy(e => e.FirstName)
            .ToListAsync();

    public async Task<IEnumerable<Employee>> SearchAsync(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return await GetAllAsync();

        var term = searchTerm.ToLower();
        return await ctx.Employees
            .Include(e => e.Position)
            .Where(e => e.FirstName.ToLower().Contains(term) || e.LastName.ToLower().Contains(term))
            .OrderBy(e => e.LastName)
            .ThenBy(e => e.FirstName)
            .ToListAsync();
    }

    public async Task<Employee> AddAsync(Employee employee)
    {
        ctx.Employees.Add(employee);
        await ctx.SaveChangesAsync();
        return employee;
    }

    public async Task<Employee> UpdateAsync(Employee employee)
    {
        ctx.Employees.Update(employee);
        await ctx.SaveChangesAsync();
        return employee;
    }

    public async Task DeleteAsync(int id)
    {
        var employee = await ctx.Employees.FindAsync(id);
        if (employee != null)
        {
            ctx.Employees.Remove(employee);
            await ctx.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsByPersonalNumberAsync(string personalNumber)
        => await ctx.Employees.AnyAsync(e => e.PersonalNumber == personalNumber);

    public async Task<bool> ExistsByEmailAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;
        
        return await ctx.Employees.AnyAsync(e => e.Email == email);
    }
}

