namespace TimeTable.ViewModels
{
    using System.Collections.Generic;
    using TimeTable.Models;

    public class Timetable2IndexViewModel
    {
        public List<Timetable2> Timetables { get; set; } = new List<Timetable2>();

        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int Limit { get; set; }

        public int? FacultyId { get; set; }
        public int? MajorId { get; set; }
        public string Section { get; set; }
        public string Semester { get; set; }
        public int? Year { get; set; }
        public string DayOfWeek { get; set; }
    }
}
