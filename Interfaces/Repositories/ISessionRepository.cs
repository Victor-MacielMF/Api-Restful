using api.Models;

namespace api.Interfaces.Repositories
{
    public interface ISessionRepository
    {
        Task<Account?> ValidateUserCredentialsAsync(string userName, string password);
    }
}