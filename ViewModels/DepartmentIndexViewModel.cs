namespace TimeTable.Models
{
    public class DepartmentIndexViewModel
    {
        public List<Department> Departments { get; set; }  // List of departments
      
        public string CodeFilter { get; set; }  // Code filter value
        public string NameFilter { get; set; }  // Name filter value
        public int Page { get; set; }
        public int Limit { get; set; }

        public int TotalCount { get; set; }
    }
}
