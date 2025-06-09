using api.Dtos;
using api.Dtos.Comment;
using api.Helpers;
using api.Interfaces.Repositories;
using api.Interfaces.Services;
using api.Models;
using api.Services;
using api.Mappers;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;

namespace Api.Tests.Services
{
    public class CommentServiceTests
    {
        private readonly ICommentRepository _commentRepository = A.Fake<ICommentRepository>();
        private readonly IStockRepository _stockRepository = A.Fake<IStockRepository>();
        private readonly ICommentService _commentService;

        public CommentServiceTests()
        {
            _commentService = new CommentService(_commentRepository, _stockRepository);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsComments_WhenExist()
        {
            var query = new QueryParameters();
            var comments = new List<Comment> 
            {
                new Comment
                {
                    Id = 1,
                    Title = "Sample Title",
                    Content = "test",
                    AccountId = "user123"
                }
            };
            A.CallTo(() => _commentRepository.GetAllAsync(query)).Returns(comments);

            var result = await _commentService.GetAllAsync(query);

            result.Title.Should().Be("Comments retrieved successfully.");
            result.Data.Should().HaveCount(1);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsMessage_WhenNoComments()
        {
            var query = new QueryParameters();
            A.CallTo(() => _commentRepository.GetAllAsync(query)).Returns(new List<Comment>());

            var result = await _commentService.GetAllAsync(query);

            result.Title.Should().Be("No comments found.");
            result.Data.Should().BeNull();
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsComment_WhenFound()
        {
            var comment = new Comment
            {
                Id = 1,
                Title = "Sample Title",
                Content = "test",
                AccountId = "user123"
            };
            A.CallTo(() => _commentRepository.GetByIdAsync(1)).Returns(comment);

            var result = await _commentService.GetByIdAsync(1);

            result.Title.Should().Be("Comment retrieved successfully.");
            result.Data.Should().NotBeNull();
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsError_WhenNotFound()
        {
            A.CallTo(() => _commentRepository.GetByIdAsync(99)).Returns((Comment?)null);

            var result = await _commentService.GetByIdAsync(99);

            result.Title.Should().Be("Comment with ID 99 not found.");
            result.Data.Should().BeNull();
        }

        [Fact]
        public async Task CreateAsync_ReturnsError_WhenDtoIsNull()
        {
            var result = await _commentService.CreateAsync(1, null, "user");

            result.Title.Should().Be("Comment data is null.");
        }

        [Fact]
        public async Task CreateAsync_ReturnsError_WhenStockDoesNotExist()
        {
            A.CallTo(() => _stockRepository.ExistsAsync(1)).Returns(false);

            var result = await _commentService.CreateAsync(1, new CreateCommentDto(), "user");

            result.Title.Should().Be("Stock with ID 1 not found.");
        }

        [Fact]
        public async Task CreateAsync_ReturnsError_WhenCreateFails()
        {
            A.CallTo(() => _stockRepository.ExistsAsync(1)).Returns(true);
            var commentDto = new CreateCommentDto { Content = "test" };
            var comment = commentDto.ToCommentFromCreateDTO(1, "user");
            A.CallTo(() => _commentRepository.CreateAsync(A<Comment>.Ignored))
                .Returns(IdentityResult.Failed(new IdentityError { Description = "Failed to create" }));

            var result = await _commentService.CreateAsync(1, commentDto, "user");

            result.Title.Should().Be("Failed to create");
            result.Data.Should().BeNull();
        }

        [Fact]
        public async Task CreateAsync_ReturnsError_WhenFetchCreatedCommentFails()
        {
            A.CallTo(() => _stockRepository.ExistsAsync(1)).Returns(true);
            var dto = new CreateCommentDto { Content = "test" };
            var comment = dto.ToCommentFromCreateDTO(1, "user");

            A.CallTo(() => _commentRepository.CreateAsync(A<Comment>._)).Returns(IdentityResult.Success);
            A.CallTo(() => _commentRepository.GetByIdAsync(A<int>._)).Returns((Comment?)null);

            var result = await _commentService.CreateAsync(1, dto, "user");

            result.Title.Should().Contain("Created comment with ID");
            result.Data.Should().BeNull();
        }

        [Fact]
        public async Task CreateAsync_ReturnsSuccess_WhenCreated()
        {
            A.CallTo(() => _stockRepository.ExistsAsync(1)).Returns(true);
            var dto = new CreateCommentDto { Content = "test" };
            var comment = dto.ToCommentFromCreateDTO(1, "user");
            var createdComment = new Comment
            {
                Id = 1,
                Title = "Sample Title",
                Content = "test",
                AccountId = "user123"
            };

            A.CallTo(() => _commentRepository.CreateAsync(A<Comment>._)).Returns(IdentityResult.Success);
            A.CallTo(() => _commentRepository.GetByIdAsync(A<int>._)).Returns(createdComment);

            var result = await _commentService.CreateAsync(1, dto, "user");

            result.Title.Should().Be("Comment created successfully.");
            result.Data.Should().NotBeNull();
        }

        [Fact]
        public async Task UpdateAsync_ReturnsError_WhenDtoIsNull()
        {
            var result = await _commentService.UpdateAsync(1, null, "user");

            result.Title.Should().Be("Comment data is null.");
        }

        [Fact]
        public async Task UpdateAsync_ReturnsError_WhenCommentNotFound()
        {
            A.CallTo(() => _commentRepository.GetByIdAsync(1)).Returns((Comment?)null);

            var result = await _commentService.UpdateAsync(1, new UpdateCommentDto(), "user");

            result.Title.Should().Be("Comment with ID 1 not found.");
        }

        [Fact]
        public async Task UpdateAsync_ReturnsError_WhenNotOwner()
        {
            var comment = new Comment { Id = 1, AccountId = "owner" };
            A.CallTo(() => _commentRepository.GetByIdAsync(1)).Returns(comment);

            var result = await _commentService.UpdateAsync(1, new UpdateCommentDto(), "not_owner");

            result.Title.Should().Be("Only the comment owner can edit it.");
        }

        [Fact]
        public async Task UpdateAsync_ReturnsError_WhenUpdateFails()
        {
            var comment = new Comment { Id = 1, AccountId = "user" };
            A.CallTo(() => _commentRepository.GetByIdAsync(1)).Returns(comment);
            A.CallTo(() => _commentRepository.UpdateAsync(A<Comment>._))
                .Returns(IdentityResult.Failed(new IdentityError { Description = "Update failed" }));

            var result = await _commentService.UpdateAsync(1, new UpdateCommentDto(), "user");

            result.Title.Should().Be("Update failed");
        }

        [Fact]
        public async Task UpdateAsync_ReturnsSuccess_WhenUpdated()
        {
            var comment = new Comment { Id = 1, AccountId = "user" };
            A.CallTo(() => _commentRepository.GetByIdAsync(1)).Returns(comment);
            A.CallTo(() => _commentRepository.UpdateAsync(A<Comment>._)).Returns(IdentityResult.Success);

            var updatedComment = new Comment { Id = 1, AccountId = "user", Content = "updated" };
            A.CallTo(() => _commentRepository.GetByIdAsync(1)).Returns(updatedComment);

            var result = await _commentService.UpdateAsync(1, new UpdateCommentDto(), "user");

            result.Title.Should().Be("Comment updated successfully.");
            result.Data.Should().NotBeNull();
        }

        [Fact]
        public async Task DeleteAsync_ReturnsError_WhenCommentNotFound()
        {
            A.CallTo(() => _commentRepository.GetByIdAsync(1)).Returns((Comment?)null);

            var result = await _commentService.DeleteAsync(1, "user");

            result.Title.Should().Be("Comment with ID 1 not found.");
        }

        [Fact]
        public async Task DeleteAsync_ReturnsError_WhenNotOwner()
        {
            var comment = new Comment { Id = 1, AccountId = "owner" };
            A.CallTo(() => _commentRepository.GetByIdAsync(1)).Returns(comment);

            var result = await _commentService.DeleteAsync(1, "not_owner");

            result.Title.Should().Be("Only the comment owner can delete it.");
        }

        [Fact]
        public async Task DeleteAsync_ReturnsError_WhenDeleteFails()
        {
            var comment = new Comment { Id = 1, AccountId = "user" };
            A.CallTo(() => _commentRepository.GetByIdAsync(1)).Returns(comment);
            A.CallTo(() => _commentRepository.DeleteAsync(comment))
                .Returns(IdentityResult.Failed(new IdentityError { Description = "Delete failed" }));

            var result = await _commentService.DeleteAsync(1, "user");

            result.Title.Should().Be("Delete failed");
        }

        [Fact]
        public async Task DeleteAsync_ReturnsSuccess_WhenDeleted()
        {
            var comment = new Comment { Id = 1, AccountId = "user", Content = "to be deleted" };
            A.CallTo(() => _commentRepository.GetByIdAsync(1)).Returns(comment);
            A.CallTo(() => _commentRepository.DeleteAsync(comment)).Returns(IdentityResult.Success);

            var result = await _commentService.DeleteAsync(1, "user");

            result.Title.Should().Be("Comment deleted successfully.");
            result.Data.Should().NotBeNull();
        }
    }
}
