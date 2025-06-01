using api.Models;

namespace api.Interfaces
{
    public interface ITokenService
    {
        (string Token, DateTime ExpiresAt) GenerateToken(Account account);
    }
}