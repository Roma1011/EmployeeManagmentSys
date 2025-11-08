# 🧩 Employee Management System  
**Clean Architecture • .NET 8 • MVC + RESTful API**

A comprehensive **Employee Management System** built using **Clean Architecture** principles, combining **ASP.NET Core MVC** with **RESTful APIs**, and designed for scalability, modularity, and maintainability.

---

## 🏗️ Architecture Overview

The system follows a layered **Clean Architecture** structure:

```
src/
├── EmployeeManagementSystem.Domain          # Domain models, business logic, validation
├── EmployeeManagementSystem.Application     # DTOs, interfaces, and services
├── EmployeeManagementSystem.Infrastructure  # EF Core, repositories, Quartz, security
└── EmployeeManagementSystem.MVC             # MVC & API controllers, views, UI
```

### **Layers**

#### 🧠 Domain Layer
- Rich domain models with encapsulated business logic and validation  
- Factory methods for creating valid domain entities  
- No anemic models — all rules enforced directly in entities  

#### 🧩 Application Layer
- DTOs, service interfaces, and high-level business services  
- Handles orchestration between UI and Domain layers  

#### ⚙️ Infrastructure Layer
- EF Core repositories  
- Quartz.NET background jobs  
- JWT authentication & BCrypt password hashing  
- Serilog logging and external service integrations  

#### 🎨 MVC Layer
- Combined MVC + RESTful API controllers  
- Views built with **Bootstrap 5**  
- Swagger UI documentation for API  

---

## 🚀 Core Features

### 👤 User Registration & Authentication
- JWT-based authentication  
- Automatic sign-in after registration  
- Password hashing using BCrypt  

### 👨‍💼 Employee Management
- Full CRUD operations  
- Validation and duplicate checks  
- Search employees by first/last name  
- Modal confirmation dialogs for deletion  

### 🏢 Position Management
- Hierarchical position tree with infinite nesting  
- Add/Delete positions dynamically  
- Parent–child relationships maintained automatically  

### ⏰ Background Jobs
- **Quartz.NET** job activates employees 1 hour after creation  

---

## 🧠 Domain Highlights
- Factory methods for entity creation  
- Encapsulated validation logic  
- Business invariants enforced at domain level  

---

## ⚙️ Technical Stack

| Area | Technology |
|------|-------------|
| Framework | .NET 8 |
| ORM | Entity Framework Core 9.0 |
| Database | SQL Server |
| Background Jobs | Quartz.NET |
| Authentication | JWT (`System.IdentityModel.Tokens.Jwt`) |
| Security | BCrypt.Net-Next |
| Logging | Serilog (Console + File) |
| API Docs | Swagger / OpenAPI |
| UI | Bootstrap 5, MVC Views |

---

## 🗄️ Database Configuration

**Connection string:**
```
Server=DESKTOP-623QCLF;Database=EmployeeManagement
```

- Initial migration created and applied using EF Core Migrations  
- Uses SQL Server for relational data storage  

---

## ✅ Validation Rules

| Field | Rule |
|--------|------|
| Personal Number | Exactly 11 characters |
| Email | Must be a valid email address |
| Registration | Email, Password, First Name, Last Name required |
| Employee | Position and Status required |
| Uniqueness | Email and Personal Number must be unique |

---

## 📦 Dependencies

- `BCrypt.Net-Next` – Password hashing  
- `System.IdentityModel.Tokens.Jwt` – JWT handling  
- `Quartz.NET` – Background job scheduler  
- `Serilog` – Structured logging  
- `Swashbuckle.AspNetCore` – Swagger integration  
- `Microsoft.EntityFrameworkCore.SqlServer` – EF Core SQL Server provider  
- `DiÆon 😎` – Custom dependency injection library   https://github.com/Roma1011/Di-on

---

## 🧰 Logging & Observability
- Structured logs written to **console** and **file**  
- Integrated **Serilog** with ASP.NET Core request pipeline  
- Supports per-request correlation and filtering  

---

## 💡 Notes

- Combines **MVC and API** within the same project for flexibility  
- Swagger UI available at:  
  ```
  /swagger
  ```
- Responsive **Bootstrap 5** layout for all pages  

---

## 🧑‍💻 Author https://github.com/Roma1011 

**Employee Management System** — Built with ❤️ using **.NET 8 Clean Architecture** 
Maintained by developers passionate about modular and maintainable enterprise solutions.


---
