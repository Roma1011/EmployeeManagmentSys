using DiÆon.Attributes;
using EmployeeManagementSystem.Domain.Entities;
using EmployeeManagementSystem.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Infrastructure.DAL.Repositories;

[Scoped]
public class UserRepository(ApplicationDbContext ctx): IUserRepository
{
    public async Task<User?> GetByEmailAsync(string email)
        => await ctx.Users.FirstOrDefaultAsync(u => u.Email == email);

    public async Task<User?> GetByIdAsync(int id)
        => await ctx.Users.FindAsync(id);

    public async Task<User> AddAsync(User user)
    {
        ctx.Users.Add(user);
        await ctx.SaveChangesAsync();
        return user;
    }

    public async Task<bool> ExistsByEmailAsync(string email)
        => await ctx.Users.AnyAsync(u => u.Email == email);
}

