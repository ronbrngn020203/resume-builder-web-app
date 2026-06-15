# ResumeBuilder Web Application

This is an ASP.NET Core MVC web application created for SOFT703 Assignment 1.
The system provides an online Resume Builder and Professional Services platform
with both Customer (Graduate) and Administrator features.

---

## ✅ Application Features (Task 2)

### 1️⃣ Web Application for Customers / Users (Graduates)
Graduates can:
- Register / Login (Authentication + Authorization)
- Browse Service Categories and view individual Services
- Start building a Resume (custom service option)
- Create, Edit, View and Delete their own Resumes
- Manage multiple resumes (CRUD)
- User-friendly navigation with role-based dashboard

> (Covers Task 2 Requirement 1.i and 1.ii)

---

### 2️⃣ Content Management System (For Admin Role)
Admins can:
- Manage Services (Add, Edit, Delete, Activate/Deactivate)
- Manage Users (Add, Edit, Delete)
- View the Admin Dashboard
- Maintain Service Categories

> (Covers Task 2 Requirement 2.i and 2.ii)

---

## ✅ Database Information
- Database Type: **SQL Server LocalDB**
- Database Name: **ResumeBuilderDB**
- Included as `.mdf` file inside project folder:  
  ➜ `App_Data/ResumeBuilderDB.mdf`
- Managed using Entity Framework Core

---

## ✅ Other Requirements (Task 3)

| Requirement | Status |
|------------|:------:|
| Master Page Layout with Header + Footer | ✅ |
| MVC Home Page using Master Page | ✅ |
| Navigation Menu w/ Role-Based Links | ✅ |
| Use of EF Core Data Controls / Views | ✅ |
| Form Validation + Error Messages | ✅ |
| Friendly Exceptions + Global Error Handler | ✅ |
| Professional Page Design (Bootstrap UI) | ✅ |
| SQL Database Included in Project | ✅ |
| N-Tier + MVC Architecture Description | ✅ |

---

## ✅ N-Tier Architecture (Task 3.9)

This MVC project uses a 3-Tier Architecture:

| Layer | MVC Component | Responsibility |
|------|---------------|----------------|
| **Presentation Layer** | Views + Controllers | Displays data and handles UI interaction |
| **Business Logic Layer** | Controllers | Processes workflow logic, validation, routing |
| **Data Access Layer** | Models + Entity Framework DB Context | Database storage and retrieval |

MVC maps into 3-Tier as follows:

- **Model** → Data Access
- **View** → Presentation
- **Controller** → Business Logic

---

## ✅ Login Credentials for Testing

| Role | Email | Password |
|------|-------|----------|
| Admin | admin@resume.com | admin123 |
| Graduate | graduate@resume.com | grad123 |

Roles determine access:
- Admin → CMS Modules (Users + Services)
- Graduate → Resume Builder + Service Browsing

---

## ✅ How to Run the Application

1. Open the solution in **Visual Studio 2022**
2. Restore NuGet Packages (automatically)
3. Ensure LocalDB instance is running
4. Run using IIS Express
5. Login using the credentials above

---

## ✅ Technologies Used
- ASP.NET Core MVC 8
- C#
- SQL Server LocalDB (.mdf)
- Entity Framework Core
- Bootstrap UI

---

## Developer Information
- Name: **Ronniel Barangan**
- Course: **SOFT703**
- Project: **ResumeBuilder Web App**
