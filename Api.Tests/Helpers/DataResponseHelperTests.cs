using api.Dtos;
using api.Helpers;
using FluentAssertions;

namespace Api.Tests.Helpers
{
    public class DataResponseHelperTests
    {
        [Fact]
        public void Error_ShouldPopulateTitleAndNullData_WhenOnlyTitleIsPassed()
        {
            // Act
            var result = DataResponseHelper.Error<string>("Something went wrong");

            // Assert
            result.Should().NotBeNull();
            result.Title.Should().Be("Something went wrong");
            result.Data.Should().BeNull();
            result.Errors.Should().BeNull();
            result.ElapsedMilliseconds.Should().BeNull();
        }

        [Fact]
        public void Error_ShouldPopulateAllFields_WhenAllArgumentsArePassed()
        {
            // Arrange
            var errors = new List<string> { "error 1", "error 2" };

            // Act
            var result = DataResponseHelper.Error<int>("Validation failed", errors, 12.5);

            // Assert
            result.Title.Should().Be("Validation failed");
            result.Errors.Should().BeEquivalentTo(errors);
            result.ElapsedMilliseconds.Should().Be(12.5);
            result.Data.Should().Be(default); // Null para int
        }
    }
}
