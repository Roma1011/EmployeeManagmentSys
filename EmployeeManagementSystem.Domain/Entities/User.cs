using EmployeeManagementSystem.Domain.Base;

namespace EmployeeManagementSystem.Domain.Entities;

public class User : BaseEntity
{
    public string   PersonalNumber { get; private set; } = string.Empty;
    public string   FirstName      { get; private set; } = string.Empty;
    public string   LastName       { get; private set; } = string.Empty;
    public Gender   Gender         { get; private set; }
    public DateTime DateOfBirth    { get; private set; }
    public string   Email          { get; private set; } = string.Empty;
    public string   PasswordHash   { get; private set; } = string.Empty;

    private User() { }

    public static User Create(
        string personalNumber,
        string firstName,
        string lastName,
        Gender gender,
        DateTime dateOfBirth,
        string email,
        string passwordHash)
    {
        ValidatePersonalNumber(personalNumber);
        ValidateName(firstName, nameof(FirstName));
        ValidateName(lastName, nameof(LastName));
        ValidateEmail(email);

        var user = new User
        {
            PersonalNumber = personalNumber,
            FirstName = firstName,
            LastName = lastName,
            Gender = gender,
            DateOfBirth = dateOfBirth,
            Email = email.ToLowerInvariant(),
            PasswordHash = passwordHash,
            CreatedAt = DateTime.UtcNow
        };

        return user;
    }

    public void UpdatePersonalInfo(string personalNumber, string firstName, string lastName, Gender gender, DateTime dateOfBirth)
    {
        ValidatePersonalNumber(personalNumber);
        ValidateName(firstName, nameof(FirstName));
        ValidateName(lastName, nameof(LastName));

        PersonalNumber = personalNumber;
        FirstName = firstName;
        LastName = lastName;
        Gender = gender;
        DateOfBirth = dateOfBirth;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateEmail(string email)
    {
        ValidateEmail(email);
        Email = email.ToLowerInvariant();
        UpdatedAt = DateTime.UtcNow;
    }

    public void ChangePassword(string newPasswordHash)
    {
        if (string.IsNullOrWhiteSpace(newPasswordHash))
            throw new ArgumentException("Password hash cannot be empty.", nameof(newPasswordHash));

        PasswordHash = newPasswordHash;
        UpdatedAt = DateTime.UtcNow;
    }

    private static void ValidatePersonalNumber(string personalNumber)
    {
        if (string.IsNullOrWhiteSpace(personalNumber))
            throw new ArgumentException("Personal number is required.", nameof(personalNumber));

        if (personalNumber.Length != 11)
            throw new ArgumentException("Personal number must be exactly 11 characters.", nameof(personalNumber));
    }

    private static void ValidateName(string name, string propertyName)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException($"{propertyName} is required.", propertyName);

        if (name.Length > 100)
            throw new ArgumentException($"{propertyName} cannot exceed 100 characters.", propertyName);
    }

    private static void ValidateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required.", nameof(email));

        if (!email.Contains('@') || !email.Contains('.'))
            throw new ArgumentException("Invalid email format.", nameof(email));

        if (email.Length > 255)
            throw new ArgumentException("Email cannot exceed 255 characters.", nameof(email));
    }
}
