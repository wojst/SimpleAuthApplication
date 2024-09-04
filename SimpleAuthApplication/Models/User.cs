using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace SimpleAuthApplication.Models
{
    public class User
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

        
        public Auth Auth { get; set; }
    }
}
