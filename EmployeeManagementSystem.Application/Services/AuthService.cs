using DiÆon.Attributes;
using EmployeeManagementSystem.Application.DTOs;
using EmployeeManagementSystem.Application.Interfaces;
using EmployeeManagementSystem.Domain.Entities;
using EmployeeManagementSystem.Domain.Interfaces;

namespace EmployeeManagementSystem.Application.Services;

[Scoped]
internal class AuthService(IUserRepository userRepository, IJwtTokenService jwtTokenService, IPasswordHasher passwordHasher): IAuthService
{
    public async Task<(bool Success, string? Token, string? ErrorMessage)> RegisterAsync(RegisterDto registerDto)
    {
        try
        {
            if (await userRepository.ExistsByEmailAsync(registerDto.Email))
                return (false, null, "Such a user already exists, please log into the system.");

            var passwordHash = passwordHasher.HashPassword(registerDto.Password);
            var user = User.Create(
                registerDto.PersonalNumber,
                registerDto.FirstName,
                registerDto.LastName,
                (Gender)registerDto.Gender,
                registerDto.DateOfBirth,
                registerDto.Email,
                passwordHash);

            await userRepository.AddAsync(user);

            var token = jwtTokenService.GenerateToken(user);
            return (true, token, null);
        }
        catch (ArgumentException ex)
        {
            return (false, null, ex.Message);
        }
        catch (Exception ex)
        {
            return (false, null, $"An error occurred during registration: {ex.Message}");
        }
    }

    public async Task<(bool Success, string? Token, string? ErrorMessage)> LoginAsync(LoginDto loginDto)
    {
        var user = await userRepository.GetByEmailAsync(loginDto.Username);
        
        if (user == null || !passwordHasher.VerifyPassword(loginDto.Password, user.PasswordHash))
            return (false, null, "Username or password is incorrect.");

        var token = jwtTokenService.GenerateToken(user);
        return (true, token, null);
    }
}
