using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeTable.Models
{
    public class Timetable2
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int ClassroomId { get; set; }

        // Navigation property for the Classroom entity
        [ForeignKey("ClassroomId")]
        public Classroom Classroom { get; set; }

        [Required]
        public int AssignCourseId { get; set; }  // Foreign key reference to AssignCourse

        // Navigation property for AssignCourse
        [ForeignKey("AssignCourseId")]
        public AssignCourse AssignCourse { get; set; }

        [Required]
        [StringLength(15)] // Optional, to limit the length of DayOfWeek values
        public string DayOfWeek { get; set; }

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        public TimeSpan EndTime { get; set; }
    }
}
