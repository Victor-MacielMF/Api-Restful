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
        private readonly ISessionRepository _sessionRepository;
        private readonly ITokenService _tokenService;

        public SessionsController(ISessionRepository sessionRepository, ITokenService tokenService)
        {
            _sessionRepository = sessionRepository;
            _tokenService = tokenService;
        }

        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(typeof(DataResponse<TokenDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Create([FromBody] LoginDto loginDto)
        {
            try
            {
                var account = await _sessionRepository.ValidateUserCredentialsAsync(loginDto.UserName, loginDto.Password);

                if (account == null)
                    return Unauthorized(new MessageResponse("Invalid username or password."));

                var (token, expiresAt) = _tokenService.GenerateToken(account);

                var authTokenDto = token.ToAuthTokenDto(expiresAt);

                return Ok(new DataResponse<TokenDto>("Authentication successful.", authTokenDto));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new MessageResponse($"An error occurred while processing your request: {ex.Message}"));
            }
        }
    }

}