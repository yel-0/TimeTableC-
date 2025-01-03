using System.ComponentModel.DataAnnotations;

namespace TimeTable.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; }
    }
}
