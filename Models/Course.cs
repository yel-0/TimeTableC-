using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TimeTable.Models
{
    public enum Semester
    {
        FirstSemester = 1,
        SecondSemester,
        ThirdSemester,
        FourthSemester,
        FifthSemester,
        SixthSemester,
        SeventhSemester,
        EighthSemester,
        NinthSemester,
        TenthSemester,
        EleventhSemester,
        TwelfthSemester,
        ThirteenthSemester,
        FourteenthSemester
    }

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

        // Adding Semester column with Enum type
        [Required]
        public Semester Semester { get; set; }
    }
}
