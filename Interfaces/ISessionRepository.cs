using api.Models;

namespace api.Interfaces
{
    public interface ISessionRepository
    {
        Task<Account?> ValidateUserCredentialsAsync(string userName, string password);
    }
}