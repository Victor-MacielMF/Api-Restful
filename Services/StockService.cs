using api.Dtos;
using api.Dtos.Stock;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Identity;

namespace api.Services
{
    public class StockService : IStockService
    {
        private readonly IStockRepository _stockRepository;

        public StockService(IStockRepository stockRepository)
        {
            _stockRepository = stockRepository;
        }

        public async Task<DataResponse<IEnumerable<StockWithoutCommentsDTO>>> GetStocks(QueryObject queryObject)
        {

            List<Stock> stocks = await _stockRepository.GetAllAsync(queryObject);
            if (stocks == null)
            {
                return new DataResponse<IEnumerable<StockWithoutCommentsDTO>>("No stocks found.");
            }
            IEnumerable<StockWithoutCommentsDTO> StocksDtos = stocks.Select(s => s.ToStockWithoutCommentsDto());

            return new DataResponse<IEnumerable<StockWithoutCommentsDTO>>("Stocks retrieved successfully.", StocksDtos);
        }

        public async Task<DataResponse<StockDto>> GetStock(int id)
        {
            Stock? stock = await _stockRepository.GetByIdAsync(id);
            if (stock == null)
            {
                return new DataResponse<StockDto>($"Stock with ID {id} not found.");
            }

            return new DataResponse<StockDto>("Stock retrieved successfully.", stock.TostockDto());
        }

        public async Task<DataResponse<StockWithoutCommentsDTO>> PostStock(CreateStockRequestDto createStockDto)
        {
            Stock? stock = createStockDto.ToStockFromCreateDTO();
            if (stock == null)
            {
                return new DataResponse<StockWithoutCommentsDTO>("Invalid stock data.");
            }
            IdentityResult result = await _stockRepository.CreateAsync(stock);

            if (result.Succeeded)
            {
                StockWithoutCommentsDTO stockDto = stock.ToStockWithoutCommentsDto();
                return new DataResponse<StockWithoutCommentsDTO>("Stock created successfully.", stockDto);
            }
            else
            {
                return new DataResponse<StockWithoutCommentsDTO>("Failed to create stock.", result.Errors.Select(e => e.Description));
            }
        }

        public async Task<DataResponse<StockWithoutCommentsDTO>> PutStock(int id, UpdateStockRequestDto stockDto)
        {
            if (stockDto == null)
                return new DataResponse<StockWithoutCommentsDTO>("Stock data is null.");

            var result = await _stockRepository.UpdateAsync(id, stockDto);

            if (!result.Succeeded)
                return new DataResponse<StockWithoutCommentsDTO>(
                    result.Errors.FirstOrDefault()?.Description ?? "Failed to update stock."
                );

            // Pega o stock atualizado para retornar no DTO
            var updatedStock = await _stockRepository.GetByIdAsync(id);
            var dto = updatedStock?.ToStockWithoutCommentsDto();

            return new DataResponse<StockWithoutCommentsDTO>("Stock updated successfully.", dto);
        }
        
        public async Task<DataResponse<string>> DeleteStockAsync(int id)
        {
            var result = await _stockRepository.DeleteAsync(id);

            if (!result.Succeeded)
                return new DataResponse<string>(
                    result.Errors.FirstOrDefault()?.Description ?? "Failed to delete stock."
                );

            return new DataResponse<string>("Stock deleted successfully.");
        }
    }
}