namespace TimeTable.ViewModels
{
    public class AssignCourseView
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public int FacultyId { get; set; }
        public string FacultyName { get; set; }

        public int MajorId { get; set; }
        public string MajorName { get; set; }
        public string Year { get; set; }
        public string Semester { get; set; }
        public string Section { get; set; }
    }

}
