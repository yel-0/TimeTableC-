using Microsoft.EntityFrameworkCore;
using TimeTable.Models;

namespace TimeTable.Data
{
    public class ApplicationDbContext : DbContext  
    {
        // Constructor that accepts DbContextOptions and passes it to the base class
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // DbSet properties representing each table in the database
        public DbSet<Major> Majors { get; set; }
        public DbSet<Classroom> Classrooms { get; set; }
        public DbSet<Faculty> Faculties { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Timetable> Timetables { get; set; }

        public DbSet<Department> Departments { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<AssignCourse> AssignCourses { get; set; }


    }
}
