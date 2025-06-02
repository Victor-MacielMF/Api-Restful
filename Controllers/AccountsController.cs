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
        private readonly IAccountService _accountService;
        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(typeof(DataResponse<AccountDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(DataResponse<IEnumerable<string>>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(DataResponse<IEnumerable<string>>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Register([FromBody] CreateAccountDto createAccountDto)
        {
            try
            {
                var result = await _accountService.RegisterAsync(createAccountDto);
                if (result.Data != null)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new DataResponse<AccountDto>("An error occurred while creating the account.", [ex.Message]));
            }
        }
    }
}