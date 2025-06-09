using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using api.Helpers;
using api.Interfaces.Services;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace api.Services
{
    public class TokenService : ITokenService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly UserManager<Account> _userManager;

        public TokenService(JwtSettings jwtSettings, UserManager<Account> userManager)
        {
            _jwtSettings = jwtSettings;
            _userManager = userManager;
        }

        public async Task<(string Token, DateTime ExpiresAt)> GenerateTokenAsync(Account account)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, account.Id),
                new Claim(ClaimTypes.GivenName, account.UserName),
                new Claim(ClaimTypes.Email, account.Email),
            };

            var roles = await _userManager.GetRolesAsync(account);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var credentials = new SigningCredentials(JwtUtils.GetSymmetricSecurityKey(_jwtSettings.SigningKey), SecurityAlgorithms.HmacSha256);
            var expiresAt = DateTime.UtcNow.AddDays(_jwtSettings.TokenValidityDays);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: expiresAt,
                signingCredentials: credentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return (tokenString, expiresAt);
        }

    }
}