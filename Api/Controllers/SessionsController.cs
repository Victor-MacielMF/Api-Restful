using api.Dtos;
using api.Dtos.Account;
using api.Interfaces.Services;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace api.Controllers
{
    [ApiController]
    [Route("api/v2/[controller]")]
    public class SessionsController : ControllerBase
    {
        private readonly ISessionService _sessionService;

        public SessionsController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(typeof(DataResponse<TokenDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(DataResponse<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(DataResponse<string>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Create([FromBody] LoginDto loginDto)
        {
            DataResponse<TokenDto> response = await _sessionService.CreateSessionAsync(loginDto);

            if (response.Errors != null)
            {
                return BadRequest(response);
            }
            else if (response.Data == null)
            {
                return NotFound(response);
            }

            return Created("/", response);
        }
    }

}