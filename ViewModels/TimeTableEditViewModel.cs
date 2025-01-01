using System.ComponentModel.DataAnnotations;

namespace TimeTable.ViewModels
{
    public class TimeTableEditViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Course is required.")]
        public int CourseId { get; set; }

        public string? CourseName { get; set; }

        [Required(ErrorMessage = "Classroom is required.")]
        public int ClassroomId { get; set; }

        public string? ClassroomName { get; set; }

        [Required(ErrorMessage = "Faculty is required.")]
        public int FacultyId { get; set; }

        public string? FacultyName { get; set; }

        // Changed to string for proper input binding in the view
        [Required(ErrorMessage = "Start time is required.")]
        public string StartTime { get; set; } = string.Empty;

        [Required(ErrorMessage = "End time is required.")]
        public string EndTime { get; set; } = string.Empty;

        [Required(ErrorMessage = "Day of the week is required.")]
        public string DayOfWeek { get; set; } = string.Empty;
    }
}
