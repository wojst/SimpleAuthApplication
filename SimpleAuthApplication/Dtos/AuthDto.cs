using System.ComponentModel.DataAnnotations;

namespace SimpleAuthApplication.Dtos
{
    public class AuthDto
    {
        [Required]
        [StringLength(50, MinimumLength = 4)]
        public string Login { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 8)]
        public string Password { get; set; }
    }
}
