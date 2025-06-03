using api.Interfaces.Repositories;
using api.Models;
using Microsoft.AspNetCore.Identity;

namespace api.Repositories
{
    public class SessionRepository : ISessionRepository
    {
        private readonly UserManager<Account> _userManager;
        private readonly SignInManager<Account> _signInManager;

        public SessionRepository(UserManager<Account> userManager, SignInManager<Account> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task<Account?> ValidateUserCredentialsAsync(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
            {
                return null;
            }

            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return null;
            }
            var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);

            return result.Succeeded ? user : null;
        }
    }
}