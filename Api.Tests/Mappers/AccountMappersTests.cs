using api.Dtos.Account;
using api.Mappers;
using api.Models;
using FluentAssertions;

namespace Api.Tests.Mappers
{
    public class AccountMappersTests
    {
        [Fact]
        public void ToAccount_ShouldMapFieldsCorrectly()
        {
            // Arrange
            var dto = new CreateAccountDto
            {
                UserName = "john_doe",
                Email = "john@example.com"
            };

            // Act
            var account = dto.ToAccount();

            // Assert
            account.Should().NotBeNull();
            account.UserName.Should().Be("john_doe");
            account.Email.Should().Be("john@example.com");
        }

        [Fact]
        public void ToAccountDto_ShouldMapFieldsCorrectly()
        {
            // Arrange
            var account = new Account
            {
                Id = "1",
                UserName = "jane_doe",
                Email = "jane@example.com"
            };

            // Act
            var dto = account.ToAccountDto();

            // Assert
            dto.Should().NotBeNull();
            dto.UserName.Should().Be("jane_doe");
            dto.Email.Should().Be("jane@example.com");
        }

        [Fact]
        public void ToAuthTokenDto_ShouldMapTokenAndExpiration()
        {
            // Arrange
            var token = "abc.123.def";
            var expiration = DateTime.UtcNow.AddHours(1);

            // Act
            var dto = token.ToAuthTokenDto(expiration);

            // Assert
            dto.Should().NotBeNull();
            dto.Token.Should().Be(token);
            dto.ExpiresAt.Should().BeCloseTo(expiration, TimeSpan.FromSeconds(1));
        }
    }
}
