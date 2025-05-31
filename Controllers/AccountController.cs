using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Account;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ITokenService _tokenService;

        public AccountController(IAccountRepository accountRepository, ITokenService tokenService)
        {
            _accountRepository = accountRepository;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] CreateAccountDto createAccountDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var account = createAccountDto.ToAccount();

                var result = await _accountRepository.CreateAsync(account, createAccountDto.Password);

                if (result.Succeeded)
                {
                    var accountDto = account.ToAccountDto();
                    accountDto.Token = _tokenService.GenerateToken(account);

                    return Ok
                    (
                        new
                        {
                            Message = "Account created successfully.",
                            Account = accountDto
                        }
                    );
                }

                return BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("login")]
        [Produces("application/json")]
        
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var account = await _accountRepository.FindByCredentialsAsync(loginDto.UserName, loginDto.Password);

                if (account == null)
                {
                    return Unauthorized(new { Message = "Invalid username or password." });
                }

                var accountDto = account.ToAccountDto();
                accountDto.Token = _tokenService.GenerateToken(account);

                return Ok(accountDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}