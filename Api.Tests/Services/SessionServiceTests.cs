using api.Dtos;
using api.Dtos.Account;
using api.Interfaces.Repositories;
using api.Interfaces.Services;
using api.Models;
using api.Services;
using api.Mappers;
using FakeItEasy;
using FluentAssertions;

namespace Api.Tests.Services
{
    public class SessionServiceTests
    {
        private readonly ISessionRepository _sessionRepository = A.Fake<ISessionRepository>();
        private readonly ITokenService _tokenService = A.Fake<ITokenService>();
        private readonly ISessionService _sessionService;

        public SessionServiceTests()
        {
            _sessionService = new SessionService(_sessionRepository, _tokenService);
        }

        [Fact]
        public async Task CreateSessionAsync_ReturnsError_WhenCredentialsAreInvalid()
        {
            // Arrange
            var loginDto = new LoginDto { Email = "user@test.com", Password = "wrongpass" };
            A.CallTo(() => _sessionRepository.ValidateCredentialsAsync(loginDto.Email, loginDto.Password))
                .Returns((Account?)null);

            // Act
            var result = await _sessionService.CreateSessionAsync(loginDto);

            // Assert
            result.Title.Should().Be("Invalid username or password.");
            result.Data.Should().BeNull();
        }

        [Fact]
        public async Task CreateSessionAsync_ReturnsToken_WhenCredentialsAreValid()
        {
            // Arrange
            var loginDto = new LoginDto { Email = "user@test.com", Password = "correctpass" };
            var account = new Account { Id = "1", Email = loginDto.Email };

            A.CallTo(() => _sessionRepository.ValidateCredentialsAsync(loginDto.Email, loginDto.Password))
                .Returns(account);

            A.CallTo(() => _tokenService.GenerateTokenAsync(account))
                .Returns(("fake-token", DateTime.UtcNow.AddHours(1)));

            // Act
            var result = await _sessionService.CreateSessionAsync(loginDto);

            // Assert
            result.Title.Should().Be("Authentication successful.");
            result.Data.Should().NotBeNull();
            result.Data.Token.Should().Be("fake-token");
        }
    }
}