using System.ComponentModel.DataAnnotations;

namespace WebHabr.Models
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Роль")]
        public string Role { get; set; }
    }
}
