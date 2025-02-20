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

        // Major ID (this will be used to populate the Major dropdown)
        public int MajorId { get; set; }

        // Major Name (this will be displayed in the input for the major search)
        public string MajorName { get; set; }
    }
}
