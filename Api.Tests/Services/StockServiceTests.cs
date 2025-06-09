using api.Dtos;
using api.Dtos.Stock;
using api.Helpers;
using api.Interfaces.Repositories;
using api.Models;
using api.Services;
using api.Mappers;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;

namespace Api.Tests.Services
{
    public class StockServiceTests
    {
        private readonly IStockRepository _stockRepository = A.Fake<IStockRepository>();
        private readonly StockService _stockService;

        public StockServiceTests()
        {
            _stockService = new StockService(_stockRepository);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsError_WhenSortFieldIsInvalid()
        {
            var query = new StockQueryParameters { SortBy = "InvalidField" };

            var result = await _stockService.GetAllAsync(query);

            result.Should().NotBeNull();
            result.Title.Should().Be("Invalid sort field: InvalidField.");
            result.Data.Should().BeNull();
            result.Errors.Should().ContainSingle();
            result.Errors.First().Should().Contain("Valid fields:");
        }

        [Fact]
        public async Task GetAllAsync_ReturnsSuccess_WhenStocksExist()
        {
            var query = new StockQueryParameters { SortBy = "CompanyName" };
            var stocks = new List<Stock>
            {
                new Stock { Id = 1, Symbol = "AAPL", CompanyName = "Apple Inc.", Purchase = 150, LastDiv = 1, Industry = "Tech", MarketCap = 2_000_000_000 }
            };

            A.CallTo(() => _stockRepository.GetAllAsync(query)).Returns(stocks);

            var result = await _stockService.GetAllAsync(query);

            result.Should().NotBeNull();
            result.Title.Should().Be("Stocks retrieved successfully.");
            result.Data.Should().HaveCount(1);
            result.Errors.Should().BeNull();

            var stock = result.Data.First();
            stock.Symbol.Should().Be("AAPL");
            stock.CompanyName.Should().Be("Apple Inc.");
            stock.Purchase.Should().Be(150);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsMessage_WhenNoStocksExist()
        {
            var query = new StockQueryParameters();
            A.CallTo(() => _stockRepository.GetAllAsync(query)).Returns(new List<Stock>());

            var result = await _stockService.GetAllAsync(query);

            result.Should().NotBeNull();
            result.Title.Should().Be("No stocks found.");
            result.Data.Should().BeNull();
            result.Errors.Should().BeNull();
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsError_WhenStockNotFound()
        {
            A.CallTo(() => _stockRepository.GetByIdAsync(1)).Returns((Stock?)null);

            var result = await _stockService.GetByIdAsync(1);

            result.Should().NotBeNull();
            result.Title.Should().Be("Stock with ID 1 not found.");
            result.Data.Should().BeNull();
            result.Errors.Should().BeNull();
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsStock_WhenFound()
        {
            var stock = new Stock
            {
                Id = 1,
                Symbol = "AAPL",
                CompanyName = "Apple",
                Purchase = 150,
                LastDiv = 1,
                Industry = "Tech",
                MarketCap = 200000000
            };

            A.CallTo(() => _stockRepository.GetByIdAsync(1)).Returns(stock);

            var result = await _stockService.GetByIdAsync(1);

            result.Should().NotBeNull();
            result.Title.Should().Be("Stock retrieved successfully.");
            result.Data.Should().NotBeNull();
            result.Data.Symbol.Should().Be("AAPL");
            result.Data.MarketCap.Should().Be(200000000);
        }

        [Fact]
        public async Task CreateAsync_ReturnsError_WhenDtoIsInvalid()
        {
            var result = await _stockService.CreateAsync(null);

            result.Should().NotBeNull();
            result.Title.Should().Be("Stock data is null.");
            result.Data.Should().BeNull();
            result.Errors.Should().BeNull();
        }

        [Fact]
        public async Task CreateAsync_ReturnsError_WhenCreateFails()
        {
            var dto = new CreateStockRequestDto
            {
                Symbol = "AAPL",
                CompanyName = "Apple",
                Purchase = 100,
                LastDiv = 1,
                Industry = "Tech",
                MarketCap = 100000000
            };

            var stock = dto.ToStockFromCreateDTO();

            A.CallTo(() => _stockRepository.CreateAsync(A<Stock>._))
                .Returns(IdentityResult.Failed(new IdentityError { Description = "DB error" }));

            var result = await _stockService.CreateAsync(dto);

            result.Should().NotBeNull();
            result.Title.Should().Be("Failed to create stock.");
            result.Data.Should().BeNull();
            result.Errors.Should().Contain("DB error");
        }

        [Fact]
        public async Task CreateAsync_ReturnsSuccess_WhenCreated()
        {
            var dto = new CreateStockRequestDto
            {
                Symbol = "AAPL",
                CompanyName = "Apple",
                Purchase = 100,
                LastDiv = 1,
                Industry = "Tech",
                MarketCap = 100000000
            };

            var stock = dto.ToStockFromCreateDTO();

            A.CallTo(() => _stockRepository.CreateAsync(A<Stock>._)).Returns(IdentityResult.Success);

            var result = await _stockService.CreateAsync(dto);

            result.Should().NotBeNull();
            result.Title.Should().Be("Stock created successfully.");
            result.Data.Should().NotBeNull();
            result.Data.Symbol.Should().Be("AAPL");
        }

        [Fact]
        public async Task UpdateAsync_ReturnsError_WhenDtoIsNull()
        {
            var result = await _stockService.UpdateAsync(1, null);

            result.Should().NotBeNull();
            result.Title.Should().Be("Stock data is null.");
            result.Data.Should().BeNull();
        }

        [Fact]
        public async Task UpdateAsync_ReturnsError_WhenUpdateFails()
        {
            var dto = new UpdateStockRequestDto
            {
                CompanyName = "Apple Updated"
            };

            A.CallTo(() => _stockRepository.UpdateAsync(1, dto))
                .Returns(IdentityResult.Failed(new IdentityError { Description = "Update failed" }));

            var result = await _stockService.UpdateAsync(1, dto);

            result.Should().NotBeNull();
            result.Title.Should().Be("Update failed");
            result.Data.Should().BeNull();
        }

        [Fact]
        public async Task UpdateAsync_ReturnsSuccess_WhenUpdated()
        {
            var dto = new UpdateStockRequestDto { CompanyName = "Apple Updated" };

            A.CallTo(() => _stockRepository.UpdateAsync(1, dto)).Returns(IdentityResult.Success);
            A.CallTo(() => _stockRepository.GetByIdAsync(1)).Returns(new Stock
            {
                Id = 1,
                Symbol = "AAPL",
                CompanyName = dto.CompanyName, // ← valor correto
                Purchase = 100,
                LastDiv = 1,
                Industry = "Tech",
                MarketCap = 100000000,
                Comments = new List<Comment>(),
                Accounts = new List<Account>()
            });

            var result = await _stockService.UpdateAsync(1, dto);

            result.Should().NotBeNull();
            result.Title.Should().Be("Stock updated successfully.");
            result.Data.Should().NotBeNull();
            result.Data.CompanyName.Should().Be("Apple Updated");
        }

        [Fact]
        public async Task DeleteAsync_ReturnsError_WhenStockNotFound()
        {
            A.CallTo(() => _stockRepository.GetByIdAsync(1)).Returns((Stock?)null);

            var result = await _stockService.DeleteAsync(1);

            result.Should().NotBeNull();
            result.Title.Should().Be("Stock with ID 1 not found.");
            result.Data.Should().BeNull();
        }

        [Fact]
        public async Task DeleteAsync_ReturnsError_WhenDeleteFails()
        {
            var stock = new Stock { Id = 1, Symbol = "AAPL" };
            A.CallTo(() => _stockRepository.GetByIdAsync(1)).Returns(stock);
            A.CallTo(() => _stockRepository.DeleteAsync(stock)).Returns(IdentityResult.Failed(new IdentityError { Description = "Delete failed" }));

            var result = await _stockService.DeleteAsync(1);

            result.Should().NotBeNull();
            result.Title.Should().Be("Failed to delete stock.");
            result.Data.Should().BeNull();
            result.Errors.Should().Contain("Delete failed");
        }

        [Fact]
        public async Task DeleteAsync_ReturnsSuccess_WhenDeleted()
        {
            var stock = new Stock
            {
                Id = 1,
                Symbol = "AAPL",
                CompanyName = "Apple"
            };

            A.CallTo(() => _stockRepository.GetByIdAsync(1)).Returns(stock);
            A.CallTo(() => _stockRepository.DeleteAsync(stock)).Returns(IdentityResult.Success);

            var result = await _stockService.DeleteAsync(1);

            result.Should().NotBeNull();
            result.Title.Should().Be("Stock deleted successfully.");
            result.Data.Should().NotBeNull();
            result.Data.Symbol.Should().Be("AAPL");
        }
    }
}
