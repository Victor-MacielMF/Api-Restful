using api.Helpers;
using FluentAssertions;

namespace Api.Tests.Helpers
{
    public class StockQueryParametersTests
    {
        [Fact]
        public void StockQueryParameters_ShouldHaveDefaultValues()
        {
            // Act
            var query = new StockQueryParameters();

            // Assert
            query.PageNumber.Should().Be(1);
            query.PageSize.Should().Be(20);
            query.Symbol.Should().BeNull();
            query.CompanyName.Should().BeNull();
            query.SortBy.Should().BeNull();
            query.IsAscending.Should().BeTrue();
        }

        [Fact]
        public void StockQueryParameters_ShouldAllowValueAssignment()
        {
            // Act
            var query = new StockQueryParameters
            {
                PageNumber = 2,
                PageSize = 10,
                Symbol = "AAPL",
                CompanyName = "Apple",
                SortBy = "MarketCap",
                IsAscending = false
            };

            // Assert
            query.PageNumber.Should().Be(2);
            query.PageSize.Should().Be(10);
            query.Symbol.Should().Be("AAPL");
            query.CompanyName.Should().Be("Apple");
            query.SortBy.Should().Be("MarketCap");
            query.IsAscending.Should().BeFalse();
        }
    }
}
