using Microsoft.AspNetCore.Mvc;
using api.Extensions;
using Microsoft.AspNetCore.Authorization;
using api.Dtos.Stock;
using api.Dtos;
using api.Interfaces.Services;

namespace api.Controllers
{
    [ApiController]
    [Route("api/v2/[controller]")]
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
        public async Task<IActionResult> GetAll()
        {
            string? username = User.GetUsername();
            DataResponse<List<StockDto>> response = await _accountStockService.GetAllAsync(username);

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
        public async Task<IActionResult> Add(int stockId)
        {
            string? username = User.GetUsername();
            DataResponse<StockDto> response = await _accountStockService.AddAsync(username, stockId);

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
        public async Task<IActionResult> Remove(int stockId)
        {
            string? username = User.GetUsername();
            DataResponse<StockDto> response = await _accountStockService.RemoveAsync(username, stockId);


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