using EmployeeManagementSystem.Application.DTOs;
using EmployeeManagementSystem.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementSystem.Mvc.Controllers;

public class EmployeesController(IEmployeeService employeeService, IPositionService positionService, ILogger<EmployeesController> logger): Controller
{
    [HttpGet]
    public async Task<IActionResult> Index(string? search)
    {
        try
        {
            IEnumerable<EmployeeDto> employees;
            if (!string.IsNullOrWhiteSpace(search))
                employees = await employeeService.SearchAsync(search);
            else
                employees = await employeeService.GetAllAsync();
            
            ViewBag.SearchTerm = search;
            return View(employees.ToList());
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error loading employees");
            TempData["ErrorMessage"] = "An error occurred while loading employees.";
            return View(new List<EmployeeDto>());
        }
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        try
        {
            var positions = await positionService.GetTreeAsync();
            ViewBag.Positions = FlattenPositions(positions.ToList());
            return View(new EmployeeDto());
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error loading positions for create");
            ViewBag.Positions = new List<PositionDto>();
            return View(new EmployeeDto());
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create(EmployeeDto employeeDto)
    {
        if (!ModelState.IsValid)
        {
            var positions = await positionService.GetTreeAsync();
            ViewBag.Positions = FlattenPositions(positions.ToList());
            return View(employeeDto);
        }

        try
        {
            var result = await employeeService.AddAsync(employeeDto);
            
            if (result.Success)
            {
                TempData["SuccessMessage"] = "Employee added successfully!";
                return RedirectToAction("Index");
            }

            if (!string.IsNullOrEmpty(result.ErrorMessage))
                ModelState.AddModelError("", result.ErrorMessage);
            else
                ModelState.AddModelError("", "Failed to add employee. Please try again.");

            var positions = await positionService.GetTreeAsync();
            ViewBag.Positions = FlattenPositions(positions.ToList());
            return View(employeeDto);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating employee");
            ModelState.AddModelError("", "An error occurred while creating the employee.");
            var positions = await positionService.GetTreeAsync();
            ViewBag.Positions = FlattenPositions(positions.ToList());
            return View(employeeDto);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            var employee = await employeeService.GetByIdAsync(id);
            if (employee == null)
            {
                TempData["ErrorMessage"] = "Employee not found.";
                return RedirectToAction("Index");
            }

            var positions = await positionService.GetTreeAsync();
            ViewBag.Positions = FlattenPositions(positions.ToList());
            return View(employee);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error loading employee for edit");
            TempData["ErrorMessage"] = "An error occurred while loading the employee.";
            return RedirectToAction("Index");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, EmployeeDto employeeDto)
    {
        if (!ModelState.IsValid)
        {
            var positions = await positionService.GetTreeAsync();
            ViewBag.Positions = FlattenPositions(positions.ToList());
            return View(employeeDto);
        }

        try
        {
            var result = await employeeService.UpdateAsync(id, employeeDto);
            
            if (result.Success)
            {
                TempData["SuccessMessage"] = "Employee updated successfully!";
                return RedirectToAction("Index");
            }

            if (!string.IsNullOrEmpty(result.ErrorMessage))
                ModelState.AddModelError("", result.ErrorMessage);
            else
                ModelState.AddModelError("", "Failed to update employee. Please try again.");

            var positions = await positionService.GetTreeAsync();
            ViewBag.Positions = FlattenPositions(positions.ToList());
            return View(employeeDto);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating employee");
            ModelState.AddModelError("", "An error occurred while updating the employee.");
            var positions = await positionService.GetTreeAsync();
            ViewBag.Positions = FlattenPositions(positions.ToList());
            return View(employeeDto);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var result = await employeeService.DeleteAsync(id);
            if (result.Success)
                TempData["SuccessMessage"] = "Employee deleted successfully!";
            else
                TempData["ErrorMessage"] = result.ErrorMessage ?? "Failed to delete employee.";
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting employee");
            TempData["ErrorMessage"] = "An error occurred while deleting the employee.";
        }

        return RedirectToAction("Index");
    }

    private List<PositionDto> FlattenPositions(List<PositionDto> positions, int level = 0)
    {
        var result = new List<PositionDto>();
        foreach (var position in positions)
        {
            result.Add(new PositionDto
            {
                Id = position.Id,
                Name = new string('─', level * 2) + (level > 0 ? " " : "") + position.Name
            });
            if (position.Children != null && position.Children.Any())
                result.AddRange(FlattenPositions(position.Children.ToList(), level + 1));
        }
        return result;
    }
}
