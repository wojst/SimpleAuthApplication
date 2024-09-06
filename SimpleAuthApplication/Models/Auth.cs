using System.ComponentModel.DataAnnotations;

namespace SimpleAuthApplication.Models
{
    public class Auth
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 4)]
        public string Login { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 8)]
        public string Password { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public Guid UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Guid UpdatedBy { get; set; }

        public User User { get; set; }
        public ICollection<Token> Tokens { get; set; }
    }
}
