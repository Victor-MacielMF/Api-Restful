using api.Models;
using Microsoft.AspNetCore.Identity;

namespace api.Interfaces.Repositories
{
    public interface IAccountRepository
    {
        Task<IdentityResult> CreateAsync(Account account, string password);
        Task<Account?> FindByUsernameAsync(string username);
        Task<Account?> GetAccountWithStocksAsync(string accountId);
    }
}