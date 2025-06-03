using api.Dtos;
using api.Dtos.Stock;
using api.Interfaces.Repositories;
using api.Interfaces.Services;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Identity;

namespace api.Services
{
    public class AccountStockService : IAccountStockService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IStockRepository _stockRepository;
        private readonly IAccountStockRepository _accountStockRepository;
        public AccountStockService(IAccountRepository accountRepository, IStockRepository stockRepository,IAccountStockRepository accountStockRepository)
        {
            _accountRepository = accountRepository;
            _stockRepository = stockRepository;
            _accountStockRepository = accountStockRepository;
        }

        public async Task<DataResponse<List<StockDto>>> GetAllAsync(string username)
        {
            Account? account = await _accountRepository.GetByUsernameAsync(username);
            if (account == null)
                return new DataResponse<List<StockDto>>("Account not found.");

            List<Stock> stocks = await _accountStockRepository.GetStocksDtoByAccountId(account);
            List<StockDto> stockDtos = stocks.Select(s => s.TostockDto()).ToList();

            return new DataResponse<List<StockDto>>("Stocks retrieved successfully.", stockDtos);
        }

        public async Task<DataResponse<StockDto>> AddAsync(string username, int stockId)
        {
            Account? account = _accountRepository.GetByUsernameAsync(username).Result;
            if (account == null)
                return new DataResponse<StockDto>("Account not found.");

            Stock? stockFromDb = await _stockRepository.GetByIdAsync(stockId);
            if (stockFromDb == null)
                return new DataResponse<StockDto>("Stock not found.");

            IdentityResult result = await _accountStockRepository.AddStockToAccountAsync(account, stockFromDb);
            if (!result.Succeeded)
                return new DataResponse<StockDto>("Failed to add stock to account.", result.Errors.Select(e => e.Description));

            StockDto stockDto = stockFromDb.TostockDto(account.Id);

            if (stockDto == null)
                return new DataResponse<StockDto>("Failed to retrieve the added stock.");

            return new DataResponse<StockDto>("Stock added to account successfully.", stockDto);
        }

        public async Task<DataResponse<StockDto>> RemoveAsync(string username, int stockId)
        {
            Account? account = await _accountRepository.GetByUsernameAsync(username);
            if (account == null)
                return new DataResponse<StockDto>("Account not found.");
            
            Stock? stockFromDb = await _stockRepository.GetByIdAsync(stockId);
            if (stockFromDb == null)
                return new DataResponse<StockDto>("Stock not found.");

            // Chama o repositório para remover
            IdentityResult result = await _accountStockRepository.RemoveStockFromAccountAsync(account, stockFromDb);
            if (!result.Succeeded)
                return new DataResponse<StockDto>(
                    result.Errors.FirstOrDefault()?.Description ?? "Failed to remove stock."
                );

            StockDto stockDto = stockFromDb.TostockDto(account.Id);

            if (stockDto == null)
                return new DataResponse<StockDto>("Failed to retrieve the removed stock.");

            return new DataResponse<StockDto>("Stock removed from account successfully.", stockDto);
        }
    }
}