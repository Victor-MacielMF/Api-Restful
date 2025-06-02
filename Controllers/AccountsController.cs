using api.Dtos;
using api.Dtos.Account;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;
        public AccountsController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        [HttpPost("register")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(DataResponse<AccountDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(DataResponse<IEnumerable<string>>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] CreateAccountDto createAccountDto)
        {
            try
            {
                var account = createAccountDto.ToAccount();

                var result = await _accountRepository.CreateAsync(account, createAccountDto.Password);

                if (result.Succeeded)
                {
                    var accountDto = account.ToAccountDto();

                    return Ok(new DataResponse<AccountDto>("Account created successfully.", accountDto));
                }

                return BadRequest(new DataResponse<IEnumerable<string>>("Error creating account.", result.Errors.Select(e => e.Description)));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new MessageResponse($"Internal server error: {ex.Message}"));
            }
        }
    }
}