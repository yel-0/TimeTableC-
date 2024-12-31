using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeTable.Models
{
    public class Timetable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int CourseId { get; set; }

        // Navigation property for the Course entity
        public Course Course { get; set; }

        [Required]
        public int ClassroomId { get; set; }

        // Navigation property for the Classroom entity
        public Classroom Classroom { get; set; }

        [Required]
        [StringLength(15)] // Optional, to limit the length of DayOfWeek values
        public string DayOfWeek { get; set; }

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        public TimeSpan EndTime { get; set; }

        [Required]
        public int Year { get; set; }

        [Required]
        public int Semester { get; set; }

        [Required]
        [StringLength(1)] 
        public string Section { get; set; }
    }
}
