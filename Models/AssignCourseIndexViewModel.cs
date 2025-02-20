namespace TimeTable.Models
{
    public class AssignCourseIndexViewModel
    {
        public List<AssignCourse> AssignCourses { get; set; }
        public int TotalCount { get; set; }
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
