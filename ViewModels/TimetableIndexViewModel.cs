﻿using TimeTable.Models;

namespace TimeTable.ViewModels
{
    public class TimetableIndexViewModel
    {
        public List<Timetable> Timetables { get; set; }
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int Limit { get; set; }

        public int? MajorId { get; set; }  // Changed to nullable

        public int? FacultyId { get; set; }  // Changed to nullable


        public string Section { get; set; }
        public int? Semester { get; set; }
        public int? Year { get; set; }
        public string DayOfWeek { get; set; }
    }
}
