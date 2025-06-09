using api.Dtos.Stock;
using api.Mappers;
using api.Models;
using FluentAssertions;

namespace Api.Tests.Mappers
{
    public class StockMappersTests
    {
        [Fact]
        public void TostockDto_WithUserId_ShouldIncludeOnlyUserComments()
        {
            // Arrange
            var stock = new Stock
            {
                Id = 1,
                Symbol = "AAPL",
                CompanyName = "Apple",
                Purchase = 150m,
                LastDiv = 1.5m,
                Industry = "Tech",
                MarketCap = 2_000_000_000,
                Comments = new List<Comment>
                {
                    new Comment { Id = 1, Content = "User 1 comment", AccountId = "user1" },
                    new Comment { Id = 2, Content = "User 2 comment", AccountId = "user2" }
                }
            };

            // Act
            var dto = stock.TostockDto("user1");

            // Assert
            dto.Should().NotBeNull();
            dto.Id.Should().Be(1);
            dto.Symbol.Should().Be("AAPL");
            dto.Comments.Should().HaveCount(1);
            dto.Comments[0].Content.Should().Be("User 1 comment");
        }

        [Fact]
        public void TostockDto_ShouldIncludeAllComments()
        {
            // Arrange
            var stock = new Stock
            {
                Id = 2,
                Symbol = "MSFT",
                CompanyName = "Microsoft",
                Purchase = 250m,
                LastDiv = 2.5m,
                Industry = "Tech",
                MarketCap = 1_000_000_000,
                Comments = new List<Comment>
                {
                    new Comment { Id = 1, Content = "Comment A", AccountId = "x" },
                    new Comment { Id = 2, Content = "Comment B", AccountId = "y" }
                }
            };

            // Act
            var dto = stock.TostockDto();

            // Assert
            dto.Should().NotBeNull();
            dto.Comments.Should().HaveCount(2);
            dto.Comments.Select(c => c.Content).Should().Contain(new[] { "Comment A", "Comment B" });
        }

        [Fact]
        public void ToStockWithoutCommentsDto_ShouldMapBasicFields()
        {
            // Arrange
            var stock = new Stock
            {
                Id = 3,
                Symbol = "GOOG",
                CompanyName = "Google",
                Purchase = 200m,
                LastDiv = 1.8m,
                Industry = "Tech",
                MarketCap = 1_500_000_000
            };

            // Act
            var dto = stock.ToStockWithoutCommentsDto();

            // Assert
            dto.Should().NotBeNull();
            dto.Symbol.Should().Be("GOOG");
            dto.CompanyName.Should().Be("Google");
            dto.Purchase.Should().Be(200m);
            dto.LastDiv.Should().Be(1.8m);
            dto.Industry.Should().Be("Tech");
            dto.MarketCap.Should().Be(1_500_000_000);
        }

        [Fact]
        public void ToStockFromCreateDTO_ShouldMapCorrectly()
        {
            // Arrange
            var dto = new CreateStockRequestDto
            {
                Symbol = "NFLX",
                CompanyName = "Netflix",
                Purchase = 180m,
                LastDiv = 1.2m,
                Industry = "Streaming",
                MarketCap = 750_000_000
            };

            // Act
            var stock = dto.ToStockFromCreateDTO();

            // Assert
            stock.Should().NotBeNull();
            stock.Symbol.Should().Be("NFLX");
            stock.CompanyName.Should().Be("Netflix");
            stock.Purchase.Should().Be(180m);
            stock.LastDiv.Should().Be(1.2m);
            stock.Industry.Should().Be("Streaming");
            stock.MarketCap.Should().Be(750_000_000);
        }
    }
}
