using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeTable.Models
{
    public class Course
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]  // Optional: Enforcing length limit
        public string CourseCode { get; set; }

        [Required]
        [StringLength(100)]  // Optional: Enforcing length limit
        public string Name { get; set; }

        public int MajorId { get; set; }

        [ForeignKey("MajorId")]
        public Major Major { get; set; }

        public int FacultyId { get; set; }

        [ForeignKey("FacultyId")]
        public Faculty Faculty { get; set; }

        [Required]
        public int Semester { get; set; } // Keeping this as an int
    }
}
