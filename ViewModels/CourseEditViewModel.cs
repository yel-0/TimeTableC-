using TimeTable.Models;

namespace TimeTable.ViewModels
{
    public class CourseEditViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CourseCode { get; set; }
        public int Semester { get; set; }
        public int MajorId { get; set; }
        public int FacultyId { get; set; }

        public string MajorName { get; set; }
        public string FacultyName { get; set; }

    }

}
