using EmployeeManagementSystem.Domain.Base;

namespace EmployeeManagementSystem.Domain.Entities;

public class Employee : BaseEntity
{
    public string         PersonalNumber { get; private set; } = string.Empty;
    public string         FirstName      { get; private set; } = string.Empty;
    public string         LastName       { get; private set; } = string.Empty;
    public Gender         Gender         { get; private set; }
    public DateTime       DateOfBirth    { get; private set; }
    public string         Email          { get; private set; } = string.Empty;
    public int            PositionId     { get; private set; }
    public Position       Position       { get; private set; } = null!;
    public EmployeeStatus Status         { get; private set; }
    public DateTime?      DismissalDate  { get; private set; }
    public bool           IsActive       { get; private set; } = false;

    private Employee() { }

    public static Employee Create(
        string personalNumber,
        string firstName,
        string lastName,
        Gender gender,
        DateTime dateOfBirth,
        string? email,
        int positionId,
        EmployeeStatus status,
        DateTime? dismissalDate = null)
    {
        ValidatePersonalNumber(personalNumber);
        ValidateName(firstName, nameof(FirstName));
        ValidateName(lastName, nameof(LastName));
        ValidateEmail(email);

        if (positionId <= 0)
            throw new ArgumentException("Position ID must be greater than zero.", nameof(positionId));

        var employee = new Employee
        {
            PersonalNumber = personalNumber,
            FirstName = firstName,
            LastName = lastName,
            Gender = gender,
            DateOfBirth = dateOfBirth,
            Email = email?.ToLowerInvariant() ?? string.Empty,
            PositionId = positionId,
            Status = status,
            DismissalDate = dismissalDate,
            IsActive = false, // Will be activated by background job after 1 hour
            CreatedAt = DateTime.UtcNow
        };

        return employee;
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

    public void UpdateEmail(string? email)
    {
        ValidateEmail(email);
        Email = email?.ToLowerInvariant() ?? string.Empty;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ChangePosition(int positionId)
    {
        if (positionId <= 0)
            throw new ArgumentException("Position ID must be greater than zero.", nameof(positionId));

        PositionId = positionId;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ChangeStatus(EmployeeStatus newStatus, DateTime? dismissalDate = null)
    {
        if (newStatus == EmployeeStatus.Dismissed && !dismissalDate.HasValue)
            throw new ArgumentException("Dismissal date is required when status is Dismissed.", nameof(dismissalDate));

        Status = newStatus;
        DismissalDate = dismissalDate;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        if (IsActive)
            throw new InvalidOperationException("Employee is already active.");

        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        if (!IsActive)
            throw new InvalidOperationException("Employee is already inactive.");

        IsActive = false;
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

    private static void ValidateEmail(string? email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return; 

        if (!email.Contains('@') || !email.Contains('.'))
            throw new ArgumentException("Invalid email format.", nameof(email));

        if (email.Length > 255)
            throw new ArgumentException("Email cannot exceed 255 characters.", nameof(email));
    }
}
