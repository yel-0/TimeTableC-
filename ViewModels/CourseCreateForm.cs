using System.ComponentModel.DataAnnotations;

namespace TimeTable.ViewModels
{
    public class CourseCreateForm
    {
        [Required]
        public string CourseCode { get; set; }

        [Required]
        public string Name { get; set; }

        [Required(ErrorMessage = "Major is required.")]
        public int MajorId { get; set; }

        [Required(ErrorMessage = "Faculty is required.")]
        public int FacultyId { get; set; }

        [Required(ErrorMessage = "Semester is required.")]
        public int Semester { get; set; }
    }
}
