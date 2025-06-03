using api.Dtos;
using api.Dtos.Account;
using api.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("api/v2/[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(typeof(DataResponse<AccountDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(DataResponse<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(DataResponse<string>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Create([FromBody] CreateAccountDto createAccountDto)
        {
            try
            {
                DataResponse<AccountDto> response = await _accountService.CreateAsync(createAccountDto);
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
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new DataResponse<AccountDto>("An error occurred while creating the account.", [ex.Message]));
            }
        }
    }
}