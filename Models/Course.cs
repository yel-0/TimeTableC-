using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TimeTable.Models
{
 

    public class Course
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string CourseCode { get; set; }

        [Required]
        public string Name { get; set; }

        public int MajorId { get; set; }

        public Major Major { get; set; }

        public int FacultyId { get; set; }

        public Faculty Faculty { get; set; }

        // Use Enum for Semester
        [Required]
        public int Semester { get; set; }
    }
}
