using TimeTable.Models;

namespace TimeTable.ViewModels
{
    public class FacultySearchViewModel
    {
        public List<Faculty> Faculties { get; set; }
        public string SearchTerm { get; set; }
        public int Limit { get; set; }
    }
}
