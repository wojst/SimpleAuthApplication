using System.ComponentModel.DataAnnotations;

namespace SimpleAuthApplication.Models
{
    public class Token
    {
        public Guid Id { get; set; }

        [Required]
        public string RefreshToken { get; set; }

        [Required]
        public DateTime RefreshTokenExpiry { get; set; }

        public Guid AuthId { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Guid UpdatedBy { get; set; }


        public Auth Auth { get; set; }
    }
}
