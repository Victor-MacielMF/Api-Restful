using api.Dtos.Comment;
using api.Mappers;
using api.Models;
using FluentAssertions;

namespace Api.Tests.Mappers
{
    public class CommentMappersTests
    {
        [Fact]
        public void ToCommentDto_ShouldMapFieldsCorrectly()
        {
            // Arrange
            var comment = new Comment
            {
                Id = 1,
                Title = "Test Title",
                Content = "Test Content",
                CreatedOn = new DateTime(2024, 1, 1),
                StockId = 2,
                Account = new Account { UserName = "john_doe" },
                AccountId = "user123" // ← obrigatório!
            };

            // Act
            var dto = comment.ToCommentDto();

            // Assert
            dto.Should().NotBeNull();
            dto.Id.Should().Be(1);
            dto.Title.Should().Be("Test Title");
            dto.Content.Should().Be("Test Content");
            dto.StockId.Should().Be(2);
            dto.CreatedBy.Should().Be("john_doe");
            dto.CreatedOn.Should().Be(comment.CreatedOn);
        }

        [Fact]
        public void ToCommentFromUpdateDTO_ShouldUpdateFields()
        {
            // Arrange
            var dto = new UpdateCommentDto
            {
                Title = "Updated Title",
                Content = "Updated Content"
            };

            var comment = new Comment
            {
                Id = 1,
                Title = "Old Title",
                Content = "Old Content",
                Account = new Account { UserName = "john_doe" },
                AccountId = "user123" // ← obrigatório!
            };

            // Act
            var result = dto.ToCommentFromUpdateDTO(comment);

            // Assert
            result.Should().NotBeNull();
            result.Title.Should().Be("Updated Title");
            result.Content.Should().Be("Updated Content");
            result.Id.Should().Be(1); // Should preserve existing ID
        }

        [Fact]
        public void ToCommentFromCreateDTO_ShouldMapAllFields()
        {
            // Arrange
            var dto = new CreateCommentDto
            {
                Title = "New Comment",
                Content = "Nice work"
            };

            int stockId = 10;
            string accountId = "user123";

            // Act
            var comment = dto.ToCommentFromCreateDTO(stockId, accountId);

            // Assert
            comment.Should().NotBeNull();
            comment.Title.Should().Be("New Comment");
            comment.Content.Should().Be("Nice work");
            comment.StockId.Should().Be(stockId);
            comment.AccountId.Should().Be(accountId);
        }
    }
}
