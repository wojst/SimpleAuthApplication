namespace SimpleAuthApplication.Models
{
    public class Token
    {
        public Guid Id { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiry { get; set; }

        
        public Guid AuthId { get; set; }
        public Auth Auth { get; set; }
    }
}
