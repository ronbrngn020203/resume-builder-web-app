# Resume Builder Web App

A full-stack web application built with ASP.NET Core 8 MVC that allows graduates to build and manage resumes, and browse professional resume services — with a separate admin panel for platform management.

---

## Features

### Graduate
- Register and log in with secure cookie-based authentication
- Create, edit, view, and delete resumes with dynamic education, experience, and skills sections
- Browse a categorized service marketplace and add services to a shopping cart

### Admin
- Manage users and services (Create, Edit, Delete, Activate/Deactivate)
- Organize services by category
- Access a dedicated admin dashboard

---

## Tech Stack
- ASP.NET Core 8 MVC
- C#
- Entity Framework Core
- SQL Server LocalDB
- Bootstrap

---

## Architecture
3-tier MVC architecture — Controllers handle business logic, Models manage data access via EF Core, and Views handle presentation.

---

## Getting Started
1. Clone the repository
2. Open `ResumeBuilderWebApp.sln` in Visual Studio 2022
3. Restore NuGet packages
4. Ensure SQL Server LocalDB is running
5. Run the project using IIS Express

### Test Credentials
| Role | Email | Password |
|------|-------|----------|
| Admin | admin@resume.com | admin123 |
| Graduate | graduate@resume.com | grad123 |
