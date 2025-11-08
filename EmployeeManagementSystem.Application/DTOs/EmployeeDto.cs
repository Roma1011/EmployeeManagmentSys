using System.ComponentModel.DataAnnotations;
using EmployeeManagementSystem.Domain.Entities;

namespace EmployeeManagementSystem.Application.DTOs;

public class EmployeeDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Personal number is required")]
    [StringLength(11, MinimumLength = 11, ErrorMessage = "Personal number must be exactly 11 characters")]
    public string PersonalNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "First name is required")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is required")]
    public string LastName { get; set; } = string.Empty;

    public Gender Gender { get; set; }

    [Required(ErrorMessage = "Date of birth is required")]
    public DateTime DateOfBirth { get; set; }

    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Position is required")]
    public int PositionId { get; set; }

    [Required(ErrorMessage = "Status is required")]
    public EmployeeStatus Status { get; set; }

    public DateTime? DismissalDate { get; set; }
}

