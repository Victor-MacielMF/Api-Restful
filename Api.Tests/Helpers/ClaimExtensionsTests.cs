using System.Security.Claims;
using api.Extensions;
using FluentAssertions;

namespace Api.Tests.Extensions
{
    public class ClaimsExtensionsTests
    {
        [Fact]
        public void GetUsername_ShouldReturnUsername_WhenClaimExists()
        {
            // Arrange
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.GivenName, "john_doe")
            };
            var identity = new ClaimsIdentity(claims);
            var user = new ClaimsPrincipal(identity);

            // Act
            var username = user.GetUsername();

            // Assert
            username.Should().Be("john_doe");
        }

        [Fact]
        public void GetUsername_ShouldThrowException_WhenClaimMissing()
        {
            // Arrange
            var user = new ClaimsPrincipal(new ClaimsIdentity());

            // Act
            Action act = () => user.GetUsername();

            // Assert
            act.Should().Throw<InvalidOperationException>()
               .WithMessage("Username claim is missing.");
        }

        [Fact]
        public void GetAccountId_ShouldReturnId_WhenClaimExists()
        {
            // Arrange
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, "123")
            };
            var identity = new ClaimsIdentity(claims);
            var user = new ClaimsPrincipal(identity);

            // Act
            var id = user.GetAccountId();

            // Assert
            id.Should().Be("123");
        }

        [Fact]
        public void GetAccountId_ShouldThrowException_WhenClaimMissing()
        {
            // Arrange
            var user = new ClaimsPrincipal(new ClaimsIdentity());

            // Act
            Action act = () => user.GetAccountId();

            // Assert
            act.Should().Throw<InvalidOperationException>()
               .WithMessage("Account ID claim is missing.");
        }
    }
}
