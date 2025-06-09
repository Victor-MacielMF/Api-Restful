using api.Models;

namespace api.Interfaces.Services
{
    public interface ITokenService
    {
        Task<(string Token, DateTime ExpiresAt)> GenerateTokenAsync(Account account);
    }
}