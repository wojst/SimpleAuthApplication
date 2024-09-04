using System.ComponentModel.DataAnnotations;

namespace SimpleAuthApplication.Dtos
{
    public class TokenDto
    {
        [Required]
        public string AccessToken { get; set; }
        [Required]
        public string RefreshToken { get; set; }
    }
}
