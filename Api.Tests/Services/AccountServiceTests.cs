using api.Dtos;
using api.Dtos.Account;
using api.Interfaces.Repositories;
using api.Interfaces.Services;
using api.Mappers;
using api.Models;
using api.Services;
using Azure;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;

namespace Api.Tests.Services
{

    public class AccountServiceTests
    {
        private readonly IAccountRepository _accountRepository = A.Fake<IAccountRepository>();

        private readonly IAccountService _accountService;
        public AccountServiceTests()
        {
            //SUT
            _accountService = new AccountService(_accountRepository);
        }

        [Fact]
        public async Task CreateAsync_ReturnsSuccessResponse_WhenAccountIsCreated()
        {
            // Arrange
            CreateAccountDto dto = new CreateAccountDto
            {
                UserName = "anyuser",
                Password = "AnyPass123!",
                Email = "any@email.com"
            };

            A.CallTo(() => _accountRepository.CreateAsync(A<Account>.Ignored, dto.Password))
                .Returns(IdentityResult.Success);

            // Act
            DataResponse<AccountDto> response = await _accountService.CreateAsync(dto);

            // Assert
            response.Should().NotBeNull();
            response.Title.Should().Be("Account created successfully.");
            response.Data.Should().NotBeNull();
            response.Errors.Should().BeNull(); // Sucesso, então não deve conter erros
            response.Data.UserName.Should().Be(dto.UserName);
            response.Data.Email.Should().Be(dto.Email);
            A.CallTo(() => _accountRepository.CreateAsync(A<Account>._, dto.Password))
            .MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task CreateAsync_ReturnsErrorResponse_WhenAccountCreationFails()
        {
            //Arrange
            CreateAccountDto dto = new CreateAccountDto
            {
                UserName = "anyuser",
                Email = "any@email.com",
                Password = "AnyPass123!"
            };
            IdentityError[] identityErrors = new[] { new IdentityError { Description = "Username is taken" } };
            IdentityResult failedResult = IdentityResult.Failed(identityErrors);

            A.CallTo(() => _accountRepository.CreateAsync(A<Account>._, dto.Password))
                .Returns(failedResult);

            //Act
            DataResponse<AccountDto> response = await _accountService.CreateAsync(dto);

            //Assert
            response.Should().NotBeNull();
            response.Title.Should().Be("Failed to create account.");
            response.Data.Should().BeNull();
            response.Errors.Should().NotBeNull();
            response.Errors.Should().Contain("Username is taken");
            A.CallTo(() => _accountRepository.CreateAsync(A<Account>._, dto.Password))
            .MustHaveHappenedOnceExactly();
        }
    }
}
