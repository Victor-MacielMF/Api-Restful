using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Identity;

namespace api.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<Account> _userManager;
        public AccountRepository(UserManager<Account> userManager)
        {
            _userManager = userManager;
        }
        public async Task<IdentityResult> CreateAsync(Account account, string password)
        {
            if (account == null || string.IsNullOrEmpty(password))
            {
                return IdentityResult.Failed(new IdentityError { Description = "Account and password cannot be null or empty." });
            }
            if (string.IsNullOrEmpty(account.UserName))
            {
                return IdentityResult.Failed(new IdentityError { Description = "Username cannot be null or empty." });
            }

            var existingUser = await _userManager.FindByNameAsync(account.UserName);
            if (existingUser != null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "User already exists." });
            }

            var result = await _userManager.CreateAsync(account, password);
            if (!result.Succeeded)
                return result;

            var roleResult = await _userManager.AddToRoleAsync(account, "User");
            if (!roleResult.Succeeded)
            {
                await _userManager.DeleteAsync(account);
                return IdentityResult.Failed(roleResult.Errors.ToArray());
            }

            return result;
        }

        public async Task<Account?> FindByUsernameAsync(string username)
        {
            return await _userManager.FindByNameAsync(username);
        }
    }
}