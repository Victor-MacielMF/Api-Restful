namespace api.Dtos.Account
{
    public class TokenDto
    {
        public string Token { get; set; } = string.Empty;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime ExpiresAt { get; set; }
    }
}