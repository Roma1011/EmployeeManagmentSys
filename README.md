# Employee Management System

A comprehensive employee management system built with Clean Architecture, .NET 8, MVC, and RESTful API.

## Features

- **User Registration & Authentication**: Secure registration and login with JWT tokens
- **Employee Management**: Full CRUD operations for employees with validation
- **Position Tree**: Hierarchical position structure with add/delete functionality
- **Employee Activation**: Automatic activation of employees 1 hour after creation (Quartz job)
- **Search Functionality**: Search employees by first name and last name
- **Structured Logging**: Serilog for comprehensive logging
- **API Documentation**: Swagger/OpenAPI for API documentation

## Architecture

The solution follows Clean Architecture principles with the following layers:

- **Domain**: Core entities and interfaces
- **Application**: Business logic, DTOs, and service interfaces
- **Infrastructure**: Data access (EF Core), repositories, Quartz jobs, and logging
- **WebApi**: RESTful API with Swagger documentation
- **Mvc**: MVC frontend consuming the API

## Prerequisites

- .NET 8 SDK
- SQL Server (LocalDB or SQL Server Express)
- Visual Studio 2022 or VS Code

## Setup Instructions

1. **Clone the repository** (if applicable) or navigate to the project directory

2. **Update Connection String**
   - Open `EmployeeManagementSystem.WebApi/appsettings.json`
   - Update the `ConnectionStrings:DefaultConnection` to match your SQL Server instance
   - Default: `Server=(localdb)\\mssqllocaldb;Database=EmployeeManagementSystem;Trusted_Connection=true;TrustServerCertificate=true`

3. **Apply Database Migrations**
   ```bash
   dotnet ef database update --project EmployeeManagementSystem.Infrastructure --startup-project EmployeeManagementSystem.WebApi
   ```

4. **Configure API Base URL** (if needed)
   - Open `EmployeeManagementSystem.Mvc/appsettings.json`
   - Update `ApiSettings:BaseUrl` to match your WebApi URL
   - Default: `https://localhost:7001/api/`

## Running the Application

### Option 1: Run Both Projects Separately

1. **Start the Web API**:
   ```bash
   cd EmployeeManagementSystem.WebApi
   dotnet run
   ```
   The API will be available at `https://localhost:7001` (or the port shown in the console)

2. **Start the MVC Application**:
   ```bash
   cd EmployeeManagementSystem.Mvc
   dotnet run
   ```
   The MVC app will be available at `https://localhost:5001` (or the port shown in the console)

### Option 2: Run from Solution Root

You can run both projects from the solution root using:
```bash
dotnet run --project EmployeeManagementSystem.WebApi
dotnet run --project EmployeeManagementSystem.Mvc
```

## API Documentation

Once the WebApi is running, access Swagger documentation at:
- `https://localhost:7001/swagger`

## Usage

1. **Register a New User**:
   - Navigate to the MVC application
   - Click "Register" or navigate to `/Auth/Register`
   - Fill in all required fields:
     - Personal Number (exactly 11 characters)
     - First Name (required)
     - Last Name (required)
     - Gender
     - Date of Birth
     - Email (required, must be valid email format)
     - Password (required, minimum 6 characters)
     - Confirm Password (must match password)
   - Upon successful registration, you'll be automatically logged in and redirected to the employee list

2. **Login**:
   - Navigate to `/Auth/Login`
   - Enter your email (username) and password
   - Upon successful login, you'll be redirected to the employee list

3. **Manage Employees**:
   - View all employees on the main page
   - Use the search box to filter by first name or last name
   - Click "Add New Employee" to create a new employee
   - Click "Edit" to modify an existing employee
   - Click "Delete" to remove an employee (with confirmation modal)

4. **Manage Positions**:
   - Navigate to "Positions" from the navigation menu
   - View the hierarchical position tree
   - Click "Add New Position" to create a new position
   - Select a parent position (optional) to create a child position
   - Click "Delete" to remove a position (cannot delete if it has children or employees)

## Validation Rules

### Registration:
- Personal number must be exactly 11 characters
- First name, last name, email, and password are required
- Email must be in valid format
- Password must be at least 6 characters
- Password confirmation must match password
- Email must be unique (cannot register with existing email)

### Employee:
- Personal number must be exactly 11 characters
- First name, last name, position, and status are required
- Email must be in valid format (if provided)
- Personal number and email must be unique
- Position must exist in the system

## Background Jobs

The system includes a Quartz job that automatically activates employees 1 hour after they are created. The job runs every minute (configurable in `DependencyInjection.cs`).

## Logging

Structured logging is configured using Serilog:
- Console output for development
- File logging in `logs/log-.txt` (daily rolling)

## Technologies Used

- .NET 8
- ASP.NET Core MVC
- ASP.NET Core Web API
- Entity Framework Core 9.0
- SQL Server
- Quartz.NET (Background Jobs)
- Serilog (Structured Logging)
- Swagger/OpenAPI
- JWT Authentication
- BCrypt (Password Hashing)
- Bootstrap 5 (UI)

## Project Structure

```
EmployeeManagementSystem/
├── EmployeeManagementSystem.Domain/
│   ├── Entities/
│   └── Interfaces/
├── EmployeeManagementSystem.Application/
│   ├── DTOs/
│   ├── Interfaces/
│   └── Services/
├── EmployeeManagementSystem.Infrastructure/
│   ├── Data/
│   ├── Repositories/
│   ├── Jobs/
│   └── DependencyInjection.cs
├── EmployeeManagementSystem.WebApi/
│   └── Controllers/
└── EmployeeManagementSystem.Mvc/
    ├── Controllers/
    ├── Views/
    └── Services/
```

## Notes

- The Quartz job is configured to run every minute for testing purposes. In production, you may want to change the cron expression in `DependencyInjection.cs` to run hourly: `"0 0 * * * ?"`
- JWT tokens are stored in session for the MVC application
- The API requires JWT authentication for all employee and position endpoints
- Make sure both the WebApi and MVC projects are running for full functionality

