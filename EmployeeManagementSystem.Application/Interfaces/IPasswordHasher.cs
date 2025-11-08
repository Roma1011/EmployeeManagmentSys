using DiÆon.Attributes;

namespace EmployeeManagementSystem.Application.Interfaces;

[Scoped]
public interface IPasswordHasher
{
    string HashPassword(string password);
    bool VerifyPassword(string password, string passwordHash);
}

