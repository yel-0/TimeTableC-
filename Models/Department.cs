using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TimeTable.Models
{
    public class Department
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }  // Primary Key

        public string Name { get; set; }  // Department Name

        public string Code { get; set; }  // Department Code (e.g., CSE for Computer Science)

        public string Description { get; set; }  // Optional description of the department

        public string Location { get; set; }  // Department Location (e.g., Building A, Room 101)

        // You can add more properties like Head of Department, etc.
    }

}
