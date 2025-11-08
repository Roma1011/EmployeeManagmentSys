using EmployeeManagementSystem.Application.DTOs;
using EmployeeManagementSystem.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementSystem.Mvc.Controllers;

public class AuthController(IAuthService authService, ILogger<AuthController> logger) : Controller
{
    [HttpGet]
    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        if (!ModelState.IsValid)
        {
            return View(loginDto);
        }

        try
        {
            var result = await authService.LoginAsync(loginDto);
            
            if (result.Success && !string.IsNullOrEmpty(result.Token))
            {
                HttpContext.Session.SetString("JWTToken", result.Token);
                return RedirectToAction("Index", "Employees");
            }

            ModelState.AddModelError("", result.ErrorMessage ?? "Username or password is incorrect.");
            return View(loginDto);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error during login");
            ModelState.AddModelError("", "An error occurred during login. Please try again.");
            return View(loginDto);
        }
    }

    [HttpGet]
    public IActionResult Register() => View();

    [HttpPost]
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {
        if (!ModelState.IsValid)
        {
            return View(registerDto);
        }

        try
        {
            var result = await authService.RegisterAsync(registerDto);
            
            if (result.Success && !string.IsNullOrEmpty(result.Token))
            {
                HttpContext.Session.SetString("JWTToken", result.Token);
                return RedirectToAction("Index", "Employees");
            }

            if (!string.IsNullOrEmpty(result.ErrorMessage))
            {
                if (result.ErrorMessage.Contains("already exists"))
                {
                    TempData["ErrorMessage"] = "Such a user already exists, please log into the system.";
                    return RedirectToAction("Login");
                }
                ModelState.AddModelError("", result.ErrorMessage);
            }
            else
            {
                ModelState.AddModelError("", "Registration failed. Please try again.");
            }

            return View(registerDto);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error during registration");
            ModelState.AddModelError("", "An error occurred during registration. Please try again.");
            return View(registerDto);
        }
    }

    [HttpPost]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }
}
