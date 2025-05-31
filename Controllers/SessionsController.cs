using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Account;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
using api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

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
        public async Task<IActionResult> Create([FromBody] LoginDto loginDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var account = await _sessionRepository.ValidateUserCredentialsAsync(loginDto.UserName, loginDto.Password);

                if (account == null)
                    return Unauthorized(new { Message = "Invalid username or password." });

                var (token, expiresAt) = _tokenService.GenerateToken(account);
                
                var authTokenDto = token.ToAuthTokenDto(expiresAt);

                return Ok(authTokenDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }

}