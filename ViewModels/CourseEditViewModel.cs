namespace TimeTable.ViewModels
{
    public class CourseEditViewModel
    {
        // Course ID
        public int Id { get; set; }

        // Course Name
        public string Name { get; set; }

        // Course Code
        public string CourseCode { get; set; }

      

        // Department ID (this will be used to populate the Department dropdown)
        public int DepartmentId { get; set; }

        // Department Name (this will be displayed in the input for the department search)
        public string DepartmentName { get; set; }
    }
}
