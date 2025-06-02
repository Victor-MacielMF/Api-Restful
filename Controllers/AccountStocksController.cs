using api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using api.Extensions;
using Microsoft.AspNetCore.Authorization;
using api.Dtos.Stock;
using api.Dtos;
using api.Mappers;
using api.Repositories;

namespace api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AccountStocksController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IAccountStockRepository _accountStockRepository;
        private readonly IStockRepository _stockRepository;
        public AccountStocksController(IAccountRepository accountRepository, IAccountStockRepository accountStockRepository, IStockRepository stockRepository)
        {
            _accountStockRepository = accountStockRepository;
            _accountRepository = accountRepository;
            _stockRepository = stockRepository;
        }

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(DataResponse<IEnumerable<StockDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> GetStocksByAccount()
        {
            var username = User.GetUsername();
            var account = await _accountRepository.FindByUsernameAsync(username);
            if (account == null)
            {
                return NotFound(new MessageResponse("Account not found."));
            }
            var stocks = await _accountStockRepository.GetStocksDtoByAccountId(account);

            return Ok(new DataResponse<IEnumerable<StockDto>>("Stocks retrieved successfully.", stocks));

        }

        [HttpPost("{stockId:int}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(DataResponse<StockDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> AddStockToAccount(int stockId)
        {
            var username = User.GetUsername();
            var account = await _accountRepository.FindByUsernameAsync(username);
            if (account == null)
            {
                return NotFound(new MessageResponse("Account not found."));
            }

            var addedStock = await _accountStockRepository.AddStockToAccountAsync(account, stockId);
            if (addedStock != null)
            {
                // REFRESH do stock para garantir que está atualizado e não tracked com outros comentários
                var stockFromDb = await _stockRepository.GetByIdAsync(stockId);

                // **Filtra os comentários pelo usuário logado usando o DTO**
                var stockDto = stockFromDb?.TostockDto(account.Id);

                if (stockDto == null)
                {
                    return BadRequest(new MessageResponse("Failed to retrieve the added stock."));
                }

                return Ok(new DataResponse<StockDto>("Stock added to account successfully.", stockDto));
            }
            return BadRequest(new MessageResponse("Stock already exists in the account."));
        }

        [HttpDelete("{stockId:int}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(DataResponse<StockDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> RemoveStockFromAccount(int stockId)
        {
            var username = User.GetUsername();
            var account = await _accountRepository.FindByUsernameAsync(username);
            if (account == null)
            {
                return NotFound(new MessageResponse("Account not found."));
            }

            var removedStock = await _accountStockRepository.RemoveStockFromAccountAsync(account, stockId);
            if (removedStock != null)
            {
                return Ok(new DataResponse<StockDto>("Stock removed from account successfully.", removedStock.TostockDto(account.Id)));
            }
            return BadRequest(new MessageResponse("Stock does not exist in the account."));
        }
    }
}