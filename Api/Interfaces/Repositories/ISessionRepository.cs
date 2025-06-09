using api.Models;

namespace api.Interfaces.Repositories
{
    public interface ISessionRepository
    {
        Task<Account?> ValidateCredentialsAsync(string email, string password);
    }
}