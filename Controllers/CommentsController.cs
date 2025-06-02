using System.Security.Claims;
using api.Dtos.Comment;
using api.Extensions;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Authorization;
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
        [ProducesResponseType(typeof(IEnumerable<CommentDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAll()
        {
            var comments = await _commentRepository.GetAllAsync();
            if (comments == null || !comments.Any())
            {
                return NotFound("No comments found.");
            }
            var commentDtos = comments.Select(c => c.ToCommentDto());

            return Ok(commentDtos);
        }

        [HttpGet("{id:int}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(CommentDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var comment = await _commentRepository.GetByIdAsync(id);
            if (comment == null)
            {
                return NotFound($"Comment with ID {id} not found.");
            }
            return Ok(comment.ToCommentDto());
        }

        [HttpPost("{stockId:int}")]
        [Produces("application/json")]
        [Authorize]
        [ProducesResponseType(typeof(CommentDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Create(int stockId, [FromBody] CreateCommentDto commentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (commentDto == null)
            {
                return BadRequest("Comment data is null.");
            }

            bool stock = await _stockRepository.ExistsAsync(stockId);
            if (stock == false)
            {
                return NotFound($"Stock with ID {stockId} not found.");
            }
            //var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var userId = User.GetAccountId();

            var comment = commentDto.ToCommentFromCreateDTO(stockId, userId);
            if (comment == null)
            {
                return BadRequest("Invalid comment data.");
            }

            var createdComment = await _commentRepository.CreateAsync(comment);
            var userComment = await _commentRepository.GetByIdAsync(createdComment.Id);

            if (userComment == null)
            {
                return NotFound($"Created comment with ID {createdComment.Id} not found.");
            }
            
            return CreatedAtAction(nameof(GetById), new { id = createdComment.Id }, userComment.ToCommentDto());

        }

        [HttpPut("{id:int}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(CommentDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCommentDto commentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (commentDto == null)
            {
                return BadRequest("Comment data is null.");
            }

            var existingComment = await _commentRepository.GetByIdAsync(id);
            if (existingComment == null)
            {
                return NotFound($"Comment with ID {id} not found.");
            }

            var updatedComment = commentDto.ToCommentFromUpdateDTO(existingComment);
            if (updatedComment == null)
            {
                return BadRequest("Invalid comment data.");
            }

            var result = await _commentRepository.UpdateAsync(updatedComment);
            return Ok(result.ToCommentDto());
        }

        [HttpDelete("{id:int}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(CommentDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var existingComment = await _commentRepository.GetByIdAsync(id);
            if (existingComment == null)
            {
                return NotFound($"Comment with ID {id} not found.");
            }

            var comment = await _commentRepository.DeleteAsync(existingComment);

            return Ok(comment.ToCommentDto());
        }
    }
}