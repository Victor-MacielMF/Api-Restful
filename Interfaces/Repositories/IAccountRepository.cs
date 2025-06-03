using api.Models;
using Microsoft.AspNetCore.Identity;

namespace api.Interfaces.Repositories
{
    public interface IAccountRepository
    {
        Task<Account?> GetByUsernameAsync(string username);
        Task<Account?> GetWithStocksByIdAsync (string accountId);
        Task<IdentityResult> CreateAsync(Account account, string password);
    }
}