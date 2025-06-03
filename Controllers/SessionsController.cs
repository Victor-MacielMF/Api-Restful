using api.Dtos;
using api.Dtos.Account;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class SessionsController : ControllerBase
    {
        private readonly ISessionService _sessionService;

        public SessionsController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(typeof(DataResponse<TokenDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(DataResponse<string>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] LoginDto loginDto)
        {
            DataResponse<TokenDto> response = await _sessionService.CreateSessionAsync(loginDto);

            if (response == null || response.Data == null)
                return BadRequest(response);

            return Ok(response);
        }
    }

}