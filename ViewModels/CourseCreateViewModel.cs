using TimeTable.Models;

namespace TimeTable.ViewModels
{
    public class CourseCreateViewModel
    {
        public List<Major> Majors { get; set; }
        public string CourseCode { get; internal set; }
        public string CourseName { get; internal set; }
        public int MajorId { get; internal set; }
    }
}
