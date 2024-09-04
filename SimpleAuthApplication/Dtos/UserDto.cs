using System.ComponentModel.DataAnnotations;

namespace SimpleAuthApplication.Dtos
{
    public class UserDto
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(100)]
        public string LastName { get; set; }
        [Required]
        [Range(1, 150)]
        public int Age { get; set; }
        [Required]
        [StringLength(100)]
        public string JobPosition { get; set; }
        [Required]
        [StringLength(50)]
        public string EmploymentType { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 4)]
        public string Login { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
