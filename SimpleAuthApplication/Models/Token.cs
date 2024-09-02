namespace SimpleAuthApplication.Models
{
    public class Token
    {
        public int Id { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiry { get; set; }

        
        public int AuthId { get; set; }
        public Auth Auth { get; set; }
    }
}
