using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeTable.Models
{
    public class AssignCourse
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int FacultyId { get; set; }

        [Required]
        public int CourseId { get; set; }

        [Required]
        public int MajorId { get; set; }  // Added MajorId

        [Required]
        public string Semester { get; set; }

        [Required]
        public string Section { get; set; }

        [Required]
        public int Year { get; set; }

        [ForeignKey("FacultyId")]
        public virtual Faculty Faculty { get; set; }

        [ForeignKey("CourseId")]
        public virtual Course Course { get; set; }

        [ForeignKey("MajorId")]
        public virtual Major Major { get; set; }  
    }
}
