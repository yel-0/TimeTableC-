using TimeTable.Models;

public class CourseIndexViewModel
{
    public List<Course> Courses { get; set; }
    public int Page { get; set; }
    public int Limit { get; set; }
    public int TotalCount { get; set; }
    public string CourseCodeFilter { get; set; }
    public string NameFilter { get; set; }

    public int? DepartmentIdFilter { get; set; }
}
