using System.ComponentModel.DataAnnotations;

namespace SimpleAuthApplication.Dtos
{
    public class UserUpdateDto
    {
        
        [StringLength(100)]
        public string FirstName { get; set; }

        
        [StringLength(100)]
        public string LastName { get; set; }

        
        [Range(1, 150)]
        public int Age { get; set; }

        
        [StringLength(100)]
        public string JobPosition { get; set; }

        
        [StringLength(50)]
        public string EmploymentType { get; set; }
    }
}
