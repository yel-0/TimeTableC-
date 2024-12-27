﻿using TimeTable.Models;
using System.Collections.Generic;

namespace TimeTable.ViewModels
{
    public class FacultyIndexViewModel
    {
        public List<Faculty> Faculties { get; set; }
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int Limit { get; set; }
        public string SearchTerm { get; set; }
    }
}