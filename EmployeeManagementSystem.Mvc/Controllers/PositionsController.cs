using EmployeeManagementSystem.Application.DTOs;
using EmployeeManagementSystem.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementSystem.Mvc.Controllers;

public class PositionsController(IPositionService positionService,ILogger<PositionsController> logger): Controller
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        try
        {
            var positions = await positionService.GetTreeAsync();
            return View(positions.ToList());
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error loading positions");
            TempData["ErrorMessage"] = "An error occurred while loading positions.";
            return View(new List<PositionDto>());
        }
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        try
        {
            var positions = await positionService.GetTreeAsync();
            ViewBag.Positions = FlattenPositions(positions.ToList());
            return View(new PositionDto());
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error loading positions for create");
            ViewBag.Positions = new List<PositionDto>();
            return View(new PositionDto());
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create(PositionDto positionDto)
    {
        if (!ModelState.IsValid)
        {
            var positions = await positionService.GetTreeAsync();
            ViewBag.Positions = FlattenPositions(positions.ToList());
            return View(positionDto);
        }

        try
        {
            var result = await positionService.AddAsync(positionDto);
            
            if (result.Success)
            {
                TempData["SuccessMessage"] = "Position added successfully!";
                return RedirectToAction("Index");
            }

            if (!string.IsNullOrEmpty(result.ErrorMessage))
            {
                ModelState.AddModelError("", result.ErrorMessage);
            }
            else
            {
                ModelState.AddModelError("", "Failed to add position. Please try again.");
            }

            var positions = await positionService.GetTreeAsync();
            ViewBag.Positions = FlattenPositions(positions.ToList());
            return View(positionDto);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating position");
            ModelState.AddModelError("", "An error occurred while creating the position.");
            var positions = await positionService.GetTreeAsync();
            ViewBag.Positions = FlattenPositions(positions.ToList());
            return View(positionDto);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var result = await positionService.DeleteAsync(id);
            if (result.Success)
            {
                TempData["SuccessMessage"] = "Position deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = result.ErrorMessage ?? "Failed to delete position. It may have children or employees assigned.";
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting position");
            TempData["ErrorMessage"] = "An error occurred while deleting the position.";
        }

        return RedirectToAction("Index");
    }

    private List<PositionDto> FlattenPositions(List<PositionDto> positions, int? parentId = null)
    {
        var result = new List<PositionDto>();
        foreach (var position in positions)
        {
            if (position.ParentId == parentId)
            {
                result.Add(position);
                if (position.Children != null && position.Children.Any())
                {
                    result.AddRange(FlattenPositions(position.Children.ToList(), position.Id));
                }
            }
        }
        return result;
    }
}
