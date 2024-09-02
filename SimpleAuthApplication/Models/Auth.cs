namespace SimpleAuthApplication.Models
{
    public class Auth
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

      
        public int UserId { get; set; }
        public User User { get; set; }

        
        public ICollection<Token> Tokens { get; set; }
    }
}
