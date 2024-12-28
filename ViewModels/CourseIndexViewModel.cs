﻿namespace TimeTable.Models
{
    public class CourseIndexViewModel
    {
        public List<Course> Courses { get; set; }
        public int TotalCourses { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string NameFilter { get; set; }

        public int? SemesterFilter { get; set; }
    }
}
