using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.Application.DTOs;

public class PositionDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Position name is required")]
    public string Name { get; set; } = string.Empty;

    public int? ParentId { get; set; }
    public string? ParentName { get; set; }
    public List<PositionDto>? Children { get; set; }
}

