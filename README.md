# 🏫 Timetable Management System

A **Timetable Management System** designed for **universities and colleges**, built using **ASP.NET and Bootstrap**. It helps administrators efficiently manage timetables, classrooms, courses, departments, and users, while allowing students and teachers to view their schedules with ease.

---

## ✨ Features

### 🔹 **Admin Capabilities**  
- Manage **Users** (Add, Edit, Delete)  
- Manage **Classrooms**  
- Manage **Majors & Departments**  
- Manage **Courses**  
- Create and Manage **Timetables**  

### 🔹 **User Capabilities**  
- View **Timetable** filtered by:  
  - **Semester & Section (A/B/C...)**  
  - **Teacher Name** (Teachers can also check their schedules)  

---

## 🛠 **Technologies Used**
- **Backend:** ASP.NET Core  
- **Frontend:** Bootstrap, Razor Views  
- **Database:** Microsoft SQL Server  
- **Session Management:** ASP.NET Session  
- **Authentication:** Session-based login  

---

## 🚀 **Getting Started**

### 1️⃣ **Clone the Repository**
```sh
git clone https://github.com/yourusername/timetable-management.git
```
```sh
git clone https://github.com/yourusername/timetable-management.git
cd timetable-management
```

### 2️⃣ **Setup the Database**
- Create a **Microsoft SQL Server** database.
- Update **`appsettings.json`** with your database connection string.
- Run migrations:
```sh
dotnet ef database update
```

### 3️⃣ **Run the Project**
```sh
dotnet run
```
