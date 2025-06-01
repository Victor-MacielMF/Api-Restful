using api.Models;
using Microsoft.AspNetCore.Identity;

namespace api.Interfaces
{
    public interface IAccountRepository
    {
        Task<IdentityResult> CreateAsync(Account account, string password);
        Task<Account?> FindByUsernameAsync(string username);
    }
}