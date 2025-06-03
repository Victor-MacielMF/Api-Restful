using System.Security.Claims;
using api.Dtos;
using api.Dtos.Comment;
using api.Extensions;
using api.Interfaces.Services;
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
        private readonly ICommentService _commentService;

        public CommentsController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(DataResponse<IEnumerable<CommentDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(DataResponse<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(DataResponse<string>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAll()
        {
            DataResponse<IEnumerable<CommentDto>> response = await _commentService.GetAllAsync();

            if (response.Errors != null)
            {
                return BadRequest(response);
            }
            else if (response.Data == null)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpGet("{id:int}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(DataResponse<CommentDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(DataResponse<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(DataResponse<string>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            DataResponse<CommentDto> response = await _commentService.GetByIdAsync(id);

            if (response.Errors != null)
            {
                return BadRequest(response);
            }
            else if (response.Data == null)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpPost("{stockId:int}")]
        [Produces("application/json")]
        [Authorize]
        [ProducesResponseType(typeof(DataResponse<CommentDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(DataResponse<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(DataResponse<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Create(int stockId, [FromBody] CreateCommentDto commentDto)
        {
            string userId = User.GetAccountId();
            DataResponse<CommentDto> response = await _commentService.CreateAsync(stockId, commentDto, userId);

            if (response.Errors != null)
            {
                return BadRequest(response);
            }
            else if (response.Data == null)
            {
                return NotFound(response);
            }

            return CreatedAtAction(nameof(GetById), new { id = response.Data?.Id }, response);
        }

        [HttpPut("{id:int}")]
        [Produces("application/json")]
        [Authorize]
        [ProducesResponseType(typeof(DataResponse<CommentDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(DataResponse<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(DataResponse<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCommentDto commentDto)
        {
            DataResponse<CommentDto> response = await _commentService.UpdateAsync(id, commentDto);

            if (response.Errors != null)
            {
                return BadRequest(response);
            }
            else if (response.Data == null)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpDelete("{id:int}")]
        [Produces("application/json")]
        [Authorize]
        [ProducesResponseType(typeof(DataResponse<CommentDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(DataResponse<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(DataResponse<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Delete(int id)
        {
            DataResponse<CommentDto> response = await _commentService.DeleteAsync(id);

            if (response.Errors != null)
            {
                return BadRequest(response);
            }
            else if (response.Data == null)
            {
                return NotFound(response);
            }

            return Ok(response);
        }
    }
}