using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using api.Helpers;
using api.Models;
using api.Services;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Api.Tests.Services
{
    public class TokenServiceTests
    {
        private readonly UserManager<Account> _userManager;
        private readonly TokenService _tokenService;

        public TokenServiceTests()
        {
            var userStore = A.Fake<IUserStore<Account>>();
            _userManager = A.Fake<UserManager<Account>>(options => options.WithArgumentsForConstructor(() =>
                new UserManager<Account>(
                    userStore,
                    null!,
                    null!,
                    null!,
                    null!,
                    null!,
                    null!,
                    null!,
                    null!
                )));

            var jwtSettings = new JwtSettings
            {
                Audience = "test-audience",
                Issuer = "test-issuer",
                SigningKey = "thisisaverysecuresecretkeywith32bytes!", // 32+ caracteres = 256+ bits
                TokenValidityDays = 7
            };

            _tokenService = new TokenService(jwtSettings, _userManager);
        }

        [Fact]
        public async Task GenerateTokenAsync_ReturnsTokenAndExpiration()
        {
            // Arrange
            var account = new Account
            {
                Id = "1",
                UserName = "testuser",
                Email = "test@example.com"
            };

            A.CallTo(() => _userManager.GetRolesAsync(account)).Returns(new List<string> { "Admin" });

            // Act
            var (token, expiresAt) = await _tokenService.GenerateTokenAsync(account);

            // Assert
            token.Should().NotBeNullOrWhiteSpace();
            expiresAt.Should().BeAfter(DateTime.UtcNow);

            // Decode token to validate claims
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);

            jwt.Claims.Should().ContainSingle(c => c.Type == ClaimTypes.NameIdentifier && c.Value == account.Id);
            jwt.Claims.Should().ContainSingle(c => c.Type == ClaimTypes.GivenName && c.Value == account.UserName);
            jwt.Claims.Should().ContainSingle(c => c.Type == ClaimTypes.Email && c.Value == account.Email);
            jwt.Claims.Should().ContainSingle(c => c.Type == ClaimTypes.Role && c.Value == "Admin");
            jwt.Issuer.Should().Be("test-issuer");
            jwt.Audiences.Should().Contain("test-audience");
        }

        [Fact]
        public async Task GenerateTokenAsync_HandlesEmptyRoles()
        {
            // Arrange
            var account = new Account
            {
                Id = "2",
                UserName = "emptyroles",
                Email = "empty@example.com"
            };

            A.CallTo(() => _userManager.GetRolesAsync(account)).Returns(new List<string>());

            // Act
            var (token, expiresAt) = await _tokenService.GenerateTokenAsync(account);

            // Assert
            token.Should().NotBeNullOrWhiteSpace();
            expiresAt.Should().BeAfter(DateTime.UtcNow);

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);

            jwt.Claims.Should().Contain(c => c.Type == ClaimTypes.NameIdentifier && c.Value == account.Id);
            jwt.Claims.Should().Contain(c => c.Type == ClaimTypes.Email && c.Value == account.Email);
            jwt.Claims.Should().NotContain(c => c.Type == ClaimTypes.Role);
        }
    }
}
