using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Identity;

namespace api.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<Account> _userManager;
        private readonly SignInManager<Account> _signInManager;
        public AccountRepository(UserManager<Account> userManager, SignInManager<Account> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task<IdentityResult> CreateAsync(Account account, string password)
        {
            if (account == null || string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException("Account and password cannot be null or empty.");
            }

            // Check if the user already exists
            var existingUser = await _userManager.FindByNameAsync(account.UserName);
            if (existingUser != null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "User already exists." });
            }

            // Create the user
            {
                var result = await _userManager.CreateAsync(account, password);
                if (result.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(account, "User");
                    if (roleResult.Succeeded)
                    {
                        return result;
                    }
                    else
                    {
                        // If adding to role fails, we can remove the user
                        await _userManager.DeleteAsync(account);
                        return IdentityResult.Failed(roleResult.Errors.ToArray());
                    }
                }
                else
                {
                    return result;
                }
            }
        }

        public async Task<Account?> FindByCredentialsAsync(string userName, string password)
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

        public async Task<Account?> FindByUsernameAsync(string username)
        {
            return await _userManager.FindByNameAsync(username) 
                   ?? throw new ArgumentException("User not found.", nameof(username));
        }
    }
}