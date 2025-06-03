using api.Models;

namespace api.Interfaces.Repositories
{
    public interface ISessionRepository
    {
        Task<Account?> ValidateCredentialsAsync(string userName, string password);
    }
}