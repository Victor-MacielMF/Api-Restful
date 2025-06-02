using System.Security.Claims;
using api.Dtos;
using api.Dtos.Comment;
using api.Extensions;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.AspNetCore.Mvc;


namespace api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IStockRepository _stockRepository;

        public CommentsController(ICommentRepository commentRepository, IStockRepository stockRepository)
        {
            _commentRepository = commentRepository;
            _stockRepository = stockRepository;
        }

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(DataResponse<IEnumerable<CommentDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAll()
        {
            var comments = await _commentRepository.GetAllAsync();
            if (comments == null || !comments.Any())
            {
                return NotFound(new MessageResponse("No comments found."));
            }
            var commentDtos = comments.Select(c => c.ToCommentDto());

            return Ok(new DataResponse<IEnumerable<CommentDto>>("Comments retrieved successfully.", commentDtos));
        }

        [HttpGet("{id:int}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(DataResponse<CommentDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var comment = await _commentRepository.GetByIdAsync(id);
            if (comment == null)
            {
                return NotFound(new MessageResponse($"Comment with ID {id} not found."));
            }
            return Ok(new DataResponse<CommentDto>("Comment retrieved successfully.", comment.ToCommentDto()));
        }

        [HttpPost("{stockId:int}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(DataResponse<CommentDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> Create(int stockId, [FromBody] CreateCommentDto commentDto)
        {
            if (commentDto == null)
            {
                return BadRequest(new MessageResponse("Comment data is null."));
            }

            bool stock = await _stockRepository.ExistsAsync(stockId);
            if (stock == false)
            {
                return NotFound(new MessageResponse($"Stock with ID {stockId} not found."));
            }
            var userId = User.GetAccountId();

            var comment = commentDto.ToCommentFromCreateDTO(stockId, userId);
            if (comment == null)
            {
                return BadRequest(new MessageResponse("Invalid comment data."));
            }

            var createdComment = await _commentRepository.CreateAsync(comment);
            var userComment = await _commentRepository.GetByIdAsync(createdComment.Id);

            if (userComment == null)
            {
                return NotFound(new MessageResponse($"Created comment with ID {createdComment.Id} not found."));
            }

            return Ok(new DataResponse<CommentDto>("Comment created successfully.", userComment.ToCommentDto()));
        }

        [HttpPut("{id:int}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(DataResponse<CommentDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCommentDto commentDto)
        {
            if (commentDto == null)
            {
                return BadRequest(new MessageResponse("Comment data is null."));
            }

            var existingComment = await _commentRepository.GetByIdAsync(id);
            if (existingComment == null)
            {
                return NotFound(new MessageResponse($"Comment with ID {id} not found."));
            }

            var updatedComment = commentDto.ToCommentFromUpdateDTO(existingComment);
            if (updatedComment == null)
            {
                return BadRequest(new MessageResponse("Invalid comment data."));
            }

            var result = await _commentRepository.UpdateAsync(updatedComment);
            return Ok(new DataResponse<CommentDto>("Comment updated successfully.", result.ToCommentDto()));
        }

        [HttpDelete("{id:int}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(DataResponse<CommentDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var existingComment = await _commentRepository.GetByIdAsync(id);
            if (existingComment == null)
            {
                return NotFound(new MessageResponse($"Comment with ID {id} not found."));
            }

            var comment = await _commentRepository.DeleteAsync(existingComment);

            return Ok(new DataResponse<CommentDto>("Comment deleted successfully.", comment.ToCommentDto()));
        }
    }
}