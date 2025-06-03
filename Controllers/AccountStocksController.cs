using Microsoft.AspNetCore.Mvc;
using api.Extensions;
using Microsoft.AspNetCore.Authorization;
using api.Dtos.Stock;
using api.Dtos;
using api.Interfaces.Services;

namespace api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AccountStocksController : ControllerBase
    {
        private readonly IAccountStockService _accountStockService;
        public AccountStocksController(IAccountStockService accountStockService)
        {
            _accountStockService = accountStockService;
        }

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(DataResponse<IEnumerable<StockDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(DataResponse<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(DataResponse<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize]
        public async Task<IActionResult> GetStocksByAccount()
        {
            string? username = User.GetUsername();
            DataResponse<List<StockDto>> response = await _accountStockService.GetStocksByAccountAsync(username);

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
        [ProducesResponseType(typeof(DataResponse<StockDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(DataResponse<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(DataResponse<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize]
        public async Task<IActionResult> AddStockToAccount(int stockId)
        {
            string? username = User.GetUsername();
            DataResponse<StockDto> response = await _accountStockService.AddStockToAccountAsync(username, stockId);

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
        
        [HttpDelete("{stockId:int}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(DataResponse<StockDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(DataResponse<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(DataResponse<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize]
        public async Task<IActionResult> RemoveStockFromAccount(int stockId)
        {
            string? username = User.GetUsername();
            DataResponse<StockDto> response = await _accountStockService.RemoveStockFromAccountAsync(username, stockId);


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