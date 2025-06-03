using api.Models;

namespace api.Interfaces.Services
{
    public interface ITokenService
    {
        (string Token, DateTime ExpiresAt) GenerateToken(Account account);
    }
}