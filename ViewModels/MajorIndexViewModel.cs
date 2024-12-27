using TimeTable.Models;
using System.Collections.Generic;

namespace TimeTable.ViewModels
{
    public class MajorIndexViewModel
    {
        public List<Major> Majors { get; set; }  // List of majors to display
        public int TotalCount { get; set; }  // Total number of majors matching the search
        public int Page { get; set; }  // Current page number
        public int Limit { get; set; }  // Number of items per page
        public string NameSearch { get; set; }  // The search term for name
    }
}
