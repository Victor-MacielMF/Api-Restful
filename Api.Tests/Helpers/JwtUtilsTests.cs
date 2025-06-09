using System.Text;
using api.Helpers;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Api.Tests.Helpers
{
    public class JwtUtilsTests
    {
        [Fact]
        public void GetSymmetricSecurityKey_FromString_ShouldReturnKey_WhenValid()
        {
            // Arrange
            var key = "supersecurekeywith32characters!!";

            // Act
            var result = JwtUtils.GetSymmetricSecurityKey(key);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<SymmetricSecurityKey>();
            Encoding.UTF8.GetString(result.Key).Should().Be(key);
        }

        [Fact]
        public void GetSymmetricSecurityKey_FromString_ShouldThrow_WhenKeyIsNullOrEmpty()
        {
            // Act
            Action actNull = () => JwtUtils.GetSymmetricSecurityKey((string?)null);
            Action actEmpty = () => JwtUtils.GetSymmetricSecurityKey("");

            // Assert
            actNull.Should().Throw<ArgumentException>()
                .WithMessage("Secret key cannot be null or empty.*");

            actEmpty.Should().Throw<ArgumentException>()
                .WithMessage("Secret key cannot be null or empty.*");
        }

        [Fact]
        public void GetSymmetricSecurityKey_FromConfig_ShouldReturnKey_WhenConfigured()
        {
            // Arrange
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    { "Jwt:SigningKey", "securekey-from-config-123456789" }
                })
                .Build();

            // Act
            var result = JwtUtils.GetSymmetricSecurityKey(config);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<SymmetricSecurityKey>();
            Encoding.UTF8.GetString(result.Key).Should().Be("securekey-from-config-123456789");
        }

        [Fact]
        public void GetSymmetricSecurityKey_FromConfig_ShouldThrow_WhenKeyMissing()
        {
            // Arrange
            var config = new ConfigurationBuilder().AddInMemoryCollection().Build();

            // Act
            Action act = () => JwtUtils.GetSymmetricSecurityKey(config);

            // Assert
            act.Should().Throw<InvalidOperationException>()
                .WithMessage("JWT secret key is not configured.");
        }
    }
}
