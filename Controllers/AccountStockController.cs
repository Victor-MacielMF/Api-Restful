using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using api.Extensions;
using Microsoft.AspNetCore.Authorization;
using api.Dtos.Stock;

namespace api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AccountStockController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IAccountStockRepository _accountStockRepository;
        public AccountStockController(IAccountRepository accountRepository, IAccountStockRepository accountStockRepository)
        {
            _accountStockRepository = accountStockRepository;
            _accountRepository = accountRepository;
        }

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<StockDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize]
        public async Task<IActionResult> GetStocksByAccount()
        {
            var username = User.GetUsername();
            var account = await _accountRepository.FindByUsernameAsync(username);
            if (account == null)
            {
                return NotFound("Account not found.");
            }
            var stocks = await _accountStockRepository.GetStocksDtoByAccountId(account);

            return Ok(stocks);
        }

        [HttpPost("{stockId:int}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> AddStockToAccount(int stockId)
        {
            var username = User.GetUsername();
            var account = await _accountRepository.FindByUsernameAsync(username);
            if (account == null)
            {
                return NotFound("Account not found.");
            }

            var result = await _accountStockRepository.AddStockToAccountAsync(account, stockId);
            if (result)
            {
                return Ok("Stock added to account successfully.");
            }
            return BadRequest("Stock already exists in the account.");
        }

        [HttpDelete("{stockId:int}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> RemoveStockFromAccount(int stockId)
        {
            var username = User.GetUsername();
            var account = await _accountRepository.FindByUsernameAsync(username);
            if (account == null)
            {
                return NotFound("Account not found.");
            }

            var result = await _accountStockRepository.RemoveStockFromAccountAsync(account, stockId);
            if (result)
            {
                return Ok("Stock removed from account successfully.");
            }
            return BadRequest("Stock does not exist in the account.");
        }
    }
}