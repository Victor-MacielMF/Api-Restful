using api.Helpers;
using api.Interfaces.Repositories;
using api.Interfaces.Services;
using api.Models;
using api.Services;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;

namespace Api.Tests.Services
{
    public class AccountStockServiceTests
    {
        private readonly IAccountRepository _accountRepository = A.Fake<IAccountRepository>();
        private readonly IStockRepository _stockRepository = A.Fake<IStockRepository>();
        private readonly IAccountStockRepository _accountStockRepository = A.Fake<IAccountStockRepository>();

        private readonly IAccountStockService _accountStockService;
        public AccountStockServiceTests()
        {
            //SUT
            _accountStockService = new AccountStockService(_accountRepository, _stockRepository,_accountStockRepository);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsStocks_WhenAccountExists()
        {
            // Arrange
            var username = "testuser";
            var account = new Account { Id = "1", UserName = username };
            var query = new QueryParameters();

            var stocks = new List<Stock>
{
                new Stock
                {
                    Id = 1,
                    Symbol = "AAPL",
                    CompanyName = "Apple Inc.",
                    Purchase = 150.00m,
                    LastDiv = 0.82m,
                    Industry = "Technology",
                    MarketCap = 2400000000000,
                    Comments = new List<Comment>(),
                    Accounts = new List<Account> { account }
                }
            };

            A.CallTo(() => _accountRepository.GetByUsernameAsync(username)).Returns(account);
            A.CallTo(() => _accountStockRepository.GetAllByAccountAsync(account, query)).Returns(stocks);

            // Act
            var result = await _accountStockService.GetAllAsync(username, query);

            // Assert
            result.Should().NotBeNull();
            result.Title.Should().Be("Stocks retrieved successfully.");
            result.Data.Should().NotBeNull();
            result.Data.Should().HaveCount(1);
            result.Errors.Should().BeNull();
        }

        [Fact]
        public async Task GetAllAsync_ReturnsError_WhenAccountNotFound()
        {            
            // Arrange
            var username = "nonexistent";
            var query = new QueryParameters();

            A.CallTo(() => _accountRepository.GetByUsernameAsync(username)).Returns((Account?)null);

            // Act
            var result = await _accountStockService.GetAllAsync(username, query);

            // Assert
            result.Should().NotBeNull();
            result.Title.Should().Be("Account not found.");
            result.Data.Should().BeNull();
            result.Errors.Should().BeNull();
        }

        [Fact]
        public async Task GetAllAsync_ReturnsEmptyList_WhenAccountHasNoStocks()
        {   
            // Arrange
            var username = "usernobroker";
            var account = new Account { Id = "2", UserName = username };
            var query = new QueryParameters();

            A.CallTo(() => _accountRepository.GetByUsernameAsync(username)).Returns(account);
            A.CallTo(() => _accountStockRepository.GetAllByAccountAsync(account, query)).Returns(new List<Stock>());

            // Act
            var result = await _accountStockService.GetAllAsync(username, query);

            // Assert
            result.Should().NotBeNull();
            result.Title.Should().Be("Stocks retrieved successfully.");
            result.Data.Should().NotBeNull();
            result.Data.Should().BeEmpty();
            result.Errors.Should().BeNull();
        }

        [Fact]
        public async Task AddAsync_ReturnsError_WhenAccountNotFound()
        {
            // Arrange
            string username = "user";
            int stockId = 1;

            A.CallTo(() => _accountRepository.GetByUsernameAsync(username)).Returns((Account?)null);

            // Act
            var result = await _accountStockService.AddAsync(username, stockId);

            // Assert
            result.Title.Should().Be("Account not found.");
            result.Data.Should().BeNull();
            result.Errors.Should().BeNull();
        }

        [Fact]
        public async Task AddAsync_ReturnsError_WhenStockNotFound()
        {
            // Arrange
            string username = "user";
            int stockId = 1;
            var account = new Account { Id = "1", UserName = username };

            A.CallTo(() => _accountRepository.GetByUsernameAsync(username)).Returns(account);
            A.CallTo(() => _stockRepository.GetByIdAsync(stockId)).Returns((Stock?)null);

            // Act
            var result = await _accountStockService.AddAsync(username, stockId);

            // Assert
            result.Title.Should().Be("Stock not found.");
            result.Data.Should().BeNull();
            result.Errors.Should().BeNull();
        }

        [Fact]
        public async Task AddAsync_ReturnsError_WhenAddFails()
        {
            // Arrange
            string username = "user";
            int stockId = 1;
            var account = new Account { Id ="1", UserName = username };
            var stock = new Stock
            {
                Id = stockId,
                Symbol = "AAPL",
                CompanyName = "Apple Inc.",
                Purchase = 150.0m,
                LastDiv = 1.5m,
                Industry = "Tech",
                MarketCap = 2500000000,
                Accounts = new List<Account> { account }
            };
            var identityErrors = new[] { new IdentityError { Description = "Some DB error" } };

            A.CallTo(() => _accountRepository.GetByUsernameAsync(username)).Returns(account);
            A.CallTo(() => _stockRepository.GetByIdAsync(stockId)).Returns(stock);
            A.CallTo(() => _accountStockRepository.AddAsync(account, stock)).Returns(IdentityResult.Failed(identityErrors));

            // Act
            var result = await _accountStockService.AddAsync(username, stockId);

            // Assert
            result.Title.Should().Be("Failed to add stock to account.");
            result.Data.Should().BeNull();
            result.Errors.Should().Contain("Some DB error");
        }

        [Fact]
        public async Task AddAsync_ReturnsSuccess_WhenStockIsAdded()
        {
            // Arrange
            string username = "user";
            int stockId = 1;
            var account = new Account { Id = "1", UserName = username };
            var stock = new Stock
            {
                Id = stockId,
                Symbol = "AAPL",
                CompanyName = "Apple Inc.",
                Purchase = 150.0m,
                LastDiv = 1.5m,
                Industry = "Tech",
                MarketCap = 2500000000,
                Accounts = new List<Account> { account }
            };

            A.CallTo(() => _accountRepository.GetByUsernameAsync(username)).Returns(account);
            A.CallTo(() => _stockRepository.GetByIdAsync(stockId)).Returns(stock);
            A.CallTo(() => _accountStockRepository.AddAsync(account, stock)).Returns(IdentityResult.Success);

            // Act
            var result = await _accountStockService.AddAsync(username, stockId);

            // Assert
            result.Title.Should().Be("Stock added to account successfully.");
            result.Data.Should().NotBeNull();
            result.Data.Symbol.Should().Be("AAPL");
            result.Errors.Should().BeNull();
        }

        [Fact]
        public async Task RemoveAsync_ReturnsError_WhenAccountNotFound()
        {
            // Arrange
            A.CallTo(() => _accountRepository.GetByUsernameAsync("anyuser")).Returns((Account?)null);

            // Act
            var result = await _accountStockService.RemoveAsync("anyuser", 1);

            // Assert
            result.Title.Should().Be("Account not found.");
            result.Data.Should().BeNull();
        }

        [Fact]
        public async Task RemoveAsync_ReturnsError_WhenStockNotFound()
        {
            // Arrange
            var account = new Account { Id = "1", UserName = "anyuser" };
            A.CallTo(() => _accountRepository.GetByUsernameAsync("anyuser")).Returns(account);
            A.CallTo(() => _stockRepository.GetByIdAsync(1)).Returns((Stock?)null);

            // Act
            var result = await _accountStockService.RemoveAsync("anyuser", 1);

            // Assert
            result.Title.Should().Be("Stock not found.");
            result.Data.Should().BeNull();
        }

        [Fact]
        public async Task RemoveAsync_ReturnsError_WhenRemoveFails()
        {
            // Arrange
            var account = new Account { Id = "1", UserName = "anyuser" };
            var stock = new Stock { Id = 1, Symbol = "AAPL", CompanyName = "Apple" };
            var identityErrors = new[] { new IdentityError { Description = "DB error" } };

            A.CallTo(() => _accountRepository.GetByUsernameAsync("anyuser")).Returns(account);
            A.CallTo(() => _stockRepository.GetByIdAsync(1)).Returns(stock);
            A.CallTo(() => _accountStockRepository.RemoveAsync(account, stock)).Returns(IdentityResult.Failed(identityErrors));

            // Act
            var result = await _accountStockService.RemoveAsync("anyuser", 1);

            // Assert
            result.Title.Should().Be("DB error");
            result.Data.Should().BeNull();
        }

        [Fact]
        public async Task RemoveAsync_ReturnsSuccess_WhenStockIsRemoved()
        {
            // Arrange
            var account = new Account { Id = "1", UserName = "anyuser" };
            var stock = new Stock
            {
                Id = 1,
                Symbol = "AAPL",
                CompanyName = "Apple",
                Purchase = 100,
                LastDiv = 2,
                Industry = "Tech",
                MarketCap = 200000000
            };

            A.CallTo(() => _accountRepository.GetByUsernameAsync("anyuser")).Returns(account);
            A.CallTo(() => _stockRepository.GetByIdAsync(1)).Returns(stock);
            A.CallTo(() => _accountStockRepository.RemoveAsync(account, stock)).Returns(IdentityResult.Success);

            // Act
            var result = await _accountStockService.RemoveAsync("anyuser", 1);

            // Assert
            result.Title.Should().Be("Stock removed from account successfully.");
            result.Data.Should().NotBeNull();
            result.Data.Symbol.Should().Be("AAPL");
        }
    }
}
