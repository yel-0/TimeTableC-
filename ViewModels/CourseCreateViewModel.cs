
using TimeTable.Models;

namespace TimeTable.ViewModels
{
    public class CourseCreateViewModel
    {
        public List<Department> Departments { get; set; }
        public string CourseCode { get; internal set; }
        public string CourseName { get; internal set; }
        public int DepartmentId { get; internal set; }
    }

}
