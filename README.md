# 🏫 Timetable Management System



The **Timetable Management System** is a web application designed to help **universities and colleges** manage timetables, classrooms, courses, departments, and user schedules. Built with **ASP.NET Core** for the backend and **Bootstrap** for the frontend, this system allows administrators to efficiently create, update, and manage timetables, while providing users (students and faculty) with an intuitive interface to view their schedules.

---

## ✨ Features

### 🔹 **Admin Capabilities**
- **Manage Users**: Add, edit, and delete users with role-based permissions (Admin, User).
- **Manage Classrooms**: Create, update, and delete classroom records.
- **Manage Majors & Departments**: Assign courses to different departments and majors.
- **Manage Courses**: Add, edit, and remove courses and link them to the relevant departments.
- **Manage Faculties**: Assign faculty members to courses.
- **Create and Manage Timetables**: Administrators can create and manage timetables, assigning classes to specific timeslots, classrooms, and teachers.

### 🔹 **User Capabilities**  
- **View Timetable**: Students and faculty can view their timetables filtered by:
  - **Semester & Section**: View schedules for specific semesters or sections (A/B/C...).
  - **Teacher Name**: Faculty members can also view their own schedules.


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

### 🔹 **Contributing**
We welcome contributions! If you would like to improve the project, please follow these steps:
1. **Fork the repository** and clone your fork locally.
2. **Create a feature branch** for your changes.
3. **Commit your changes** with clear and concise messages.
4. **Submit a pull request** describing your changes.


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
