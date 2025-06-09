using api.Helpers;
using FluentAssertions;

namespace Api.Tests.Helpers
{
    public class QueryParametersTests
    {
        [Fact]
        public void QueryParameters_ShouldHaveDefaultValues()
        {
            // Act
            var query = new QueryParameters();

            // Assert
            query.PageNumber.Should().Be(1);
            query.PageSize.Should().Be(20);
        }

        [Fact]
        public void QueryParameters_ShouldAllowValueAssignment()
        {
            // Act
            var query = new QueryParameters
            {
                PageNumber = 3,
                PageSize = 50
            };

            // Assert
            query.PageNumber.Should().Be(3);
            query.PageSize.Should().Be(50);
        }
    }
}
