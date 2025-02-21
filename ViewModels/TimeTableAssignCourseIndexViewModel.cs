using TimeTable.Models;

namespace TimeTable.ViewModels
{
    public class TimeTableAssignCourseIndexViewModel
    {
        public List<AssignCourse> AssignCourses { get; set; }
        public int TotalAssignCount { get; set; }
        public List<Timetable2> TimetableEntries { get; set; }
        public int TotalTimetableCount { get; set; }
        public int Page { get; set; }
        public int Limit { get; set; }
        public int? FacultyId { get; set; }
        public int? CourseId { get; set; }
        public int? MajorId { get; set; }
        public string Section { get; set; }
        public string Semester { get; set; }
        public int? Year { get; set; }
    }
}
