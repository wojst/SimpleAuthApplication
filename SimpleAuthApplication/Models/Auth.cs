namespace SimpleAuthApplication.Models
{
    public class Auth
    {
        public Guid Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

      
        public Guid UserId { get; set; }
        public User User { get; set; }

        
        public ICollection<Token> Tokens { get; set; }
    }
}
