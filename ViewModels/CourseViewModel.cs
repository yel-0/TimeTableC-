namespace TimeTable.Models
{
    public class CourseViewModel
    {
        public List<Course> Courses { get; set; }
        public int TotalCourses { get; set; }
        public int Page { get; set; }
        public int Limit { get; set; }
        public Semester? Semester { get; set; }
    }
}
