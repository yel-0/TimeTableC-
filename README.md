# 🏫 Timetable Management System

A **Timetable Management System** designed for **universities and colleges**, built using **ASP.NET and Bootstrap**. It helps administrators efficiently manage timetables, classrooms, courses, departments, and users, while allowing students and teachers to view their schedules with ease.

---

## ✨ Features

### 🔹 **Admin Capabilities**  
- Manage **Users** (Add, Edit, Delete)  
- Manage **Classrooms**  
- Manage **Majors & Departments**  
- Manage **Courses**
- Manage **Faculties**  
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
git clone https://github.com/yel-0/TimeTableC-.git
```
```sh
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

### 📝 Contributing

If you'd like to contribute, please fork the repository and submit a pull request. Contributions, bug reports, and feature requests are always welcome!

### 📞 Contact

For any issues or queries, feel free to reach out to me at awin4989@gmail.com.

### 👨‍💻 Author

Developed by Yel.

### 🐛 Known Issues and TODOs
### Known Issues:
- **Excel File Upload Integration**: Currently, there is no integration for uploading and importing data from Excel files.
- **User Feedback**: Success and error messages are not yet implemented for user interactions (e.g., form submissions, timetable creation).
  
### TODOs:
- **Excel File Upload**: Implement the ability to import timetables and user data from Excel files for bulk uploads.
- **User Feedback**: Add success and error messages to improve user experience, such as confirming form submissions or action completions.
