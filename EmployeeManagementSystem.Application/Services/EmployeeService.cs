using DiÆon.Attributes;
using EmployeeManagementSystem.Application.DTOs;
using EmployeeManagementSystem.Application.Interfaces;
using EmployeeManagementSystem.Domain.Entities;
using EmployeeManagementSystem.Domain.Interfaces;

namespace EmployeeManagementSystem.Application.Services;

[Scoped]
internal class EmployeeService(IEmployeeRepository employeeRepository,IPositionRepository positionRepository): IEmployeeService
{
    public async Task<IEnumerable<EmployeeDto>> GetAllAsync()
    {
        var employees = await employeeRepository.GetAllAsync();
        return employees.Select(MapToDto);
    }

    public async Task<IEnumerable<EmployeeDto>> SearchAsync(string searchTerm)
    {
        var employees = await employeeRepository.SearchAsync(searchTerm);
        return employees.Select(MapToDto);
    }

    public async Task<EmployeeDto?> GetByIdAsync(int id)
    {
        var employee = await employeeRepository.GetByIdAsync(id);
        return employee != null ? MapToDto(employee) : null;
    }

    public async Task<(bool Success, string? ErrorMessage)> AddAsync(EmployeeDto employeeDto)
    {
        try
        {
            if (await employeeRepository.ExistsByPersonalNumberAsync(employeeDto.PersonalNumber))
                return (false, "An employee with this personal number already exists");

            if (!string.IsNullOrEmpty(employeeDto.Email) && await employeeRepository.ExistsByEmailAsync(employeeDto.Email))
                return (false, "An employee with this email already exists");

            var position = await positionRepository.GetByIdAsync(employeeDto.PositionId);
            if (position == null)
                return (false, "Position not found");

            var employee = Employee.Create(
                employeeDto.PersonalNumber,
                employeeDto.FirstName,
                employeeDto.LastName,
                employeeDto.Gender,
                employeeDto.DateOfBirth,
                employeeDto.Email,
                employeeDto.PositionId,
                employeeDto.Status,
                employeeDto.DismissalDate);

            await employeeRepository.AddAsync(employee);
            return (true, null);
        }
        catch (ArgumentException ex)
        {
            return (false, ex.Message);
        }
        catch (Exception ex)
        {
            return (false, $"An error occurred while adding the employee: {ex.Message}");
        }
    }

    public async Task<(bool Success, string? ErrorMessage)> UpdateAsync(int id, EmployeeDto employeeDto)
    {
        try
        {
            var employee = await employeeRepository.GetByIdAsync(id);
            if (employee == null)
                return (false, "Employee not found");

            if (employeeDto.PersonalNumber != employee.PersonalNumber && 
                await employeeRepository.ExistsByPersonalNumberAsync(employeeDto.PersonalNumber))
                return (false, "An employee with this personal number already exists");

            if (!string.IsNullOrEmpty(employeeDto.Email) && 
                employeeDto.Email != employee.Email && 
                await employeeRepository.ExistsByEmailAsync(employeeDto.Email)) 
                return (false, "An employee with this email already exists");

            var position = await positionRepository.GetByIdAsync(employeeDto.PositionId);
            if (position == null)
                return (false, "Position not found");

            employee.UpdatePersonalInfo(
                employeeDto.PersonalNumber,
                employeeDto.FirstName,
                employeeDto.LastName,
                employeeDto.Gender,
                employeeDto.DateOfBirth);

            employee.UpdateEmail(employeeDto.Email);
            employee.ChangePosition(employeeDto.PositionId);
            employee.ChangeStatus(employeeDto.Status, employeeDto.DismissalDate);

            await employeeRepository.UpdateAsync(employee);
            return (true, null);
        }
        catch (ArgumentException ex)
        {
            return (false, ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return (false, ex.Message);
        }
        catch (Exception ex)
        {
            return (false, $"An error occurred while updating the employee: {ex.Message}");
        }
    }

    public async Task<(bool Success, string? ErrorMessage)> DeleteAsync(int id)
    {
        var employee = await employeeRepository.GetByIdAsync(id);
        if (employee == null)
            return (false, "Employee not found");

        await employeeRepository.DeleteAsync(id);
        return (true, null);
    }

    private static EmployeeDto MapToDto(Employee employee)
    {
        return new EmployeeDto
        {
            Id = employee.Id,
            PersonalNumber = employee.PersonalNumber,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            Gender = employee.Gender,
            DateOfBirth = employee.DateOfBirth,
            Email = employee.Email,
            PositionId = employee.PositionId,
            Status = employee.Status,
            DismissalDate = employee.DismissalDate
        };
    }
}
