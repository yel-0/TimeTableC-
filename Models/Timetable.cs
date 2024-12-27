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

        public int CourseId { get; set; }

        public Course Course { get; set; }

        public int ClassroomId { get; set; }

        public Classroom Classroom { get; set; } 

        [Required]
        public string DayOfWeek { get; set; }

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        public TimeSpan EndTime { get; set; }

        [Required]
        public string Semester { get; set; }  // e.g., "First Semester", "Second Semester"

        [Required]
        public int Year { get; set; }
    }
}
