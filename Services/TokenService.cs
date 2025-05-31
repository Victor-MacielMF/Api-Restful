using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using api.Dtos.Account;
using api.Helpers;
using api.Interfaces;
using api.Models;
using Microsoft.IdentityModel.Tokens;

namespace api.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly SymmetricSecurityKey _signingKey;
        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
            _signingKey = JwtUtils.GetSymmetricSecurityKey(configuration);
        }

        public string GenerateToken(Account account)
        {
            if (string.IsNullOrEmpty(account.Id))
                throw new ArgumentException("User ID cannot be null or empty.", nameof(account.Id));
            if (string.IsNullOrEmpty(account.UserName))
                throw new ArgumentException("User name cannot be null or empty.", nameof(account.UserName));

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, account.Id),
                new Claim(ClaimTypes.GivenName, account.UserName),
                new Claim(ClaimTypes.Email, account.Email),
            };

            var credentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}