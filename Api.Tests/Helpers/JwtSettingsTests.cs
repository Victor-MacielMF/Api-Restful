using api.Helpers;
using FluentAssertions;

namespace Api.Tests.Helpers
{
    public class JwtSettingsTests
    {
        [Fact]
        public void JwtSettings_ShouldStoreValuesCorrectly()
        {
            // Arrange
            var settings = new JwtSettings
            {
                Issuer = "my-issuer",
                Audience = "my-audience",
                SigningKey = "supersecurekey12345678901234567890",
                TokenValidityDays = 5
            };

            // Assert
            settings.Issuer.Should().Be("my-issuer");
            settings.Audience.Should().Be("my-audience");
            settings.SigningKey.Should().Be("supersecurekey12345678901234567890");
            settings.TokenValidityDays.Should().Be(5);
        }
    }
}
