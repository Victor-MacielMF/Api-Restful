using api.Dtos;
using api.Dtos.Stock;
using api.Helpers;
using api.Interfaces.Repositories;
using api.Interfaces.Services;
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

        private static readonly HashSet<string> SortableFields = new()
        {
            "Id",
            "Symbol",
            "CompanyName",
            "Purchase",
            "LastDiv",
            "Industry",
            "MarketCap"
        };

        public async Task<DataResponse<IEnumerable<StockWithoutCommentsDTO>>> GetAllAsync(QueryObject queryObject)
        {
            // Campos válidos para ordenação
            var sortableFields = SortableFields;

            // Validação do SortBy
            if (!string.IsNullOrEmpty(queryObject.SortBy) && !sortableFields.Contains(queryObject.SortBy))
            {
                return new DataResponse<IEnumerable<StockWithoutCommentsDTO>>(
                    $"Invalid sort field: {queryObject.SortBy}.",
                    new List<string> { "Valid fields: " + string.Join(", ", sortableFields) }
                );
            }

            // Ajusta SortBy para nome correto (capitalização)
            if (!string.IsNullOrEmpty(queryObject.SortBy))
            {
                queryObject.SortBy = sortableFields.First(f => f.Equals(queryObject.SortBy, StringComparison.OrdinalIgnoreCase));
            }

            List<Stock> stocks = await _stockRepository.GetAllAsync(queryObject);

            if (stocks == null || !stocks.Any())
            {
                return new DataResponse<IEnumerable<StockWithoutCommentsDTO>>("No stocks found.");
            }

            var StocksDtos = stocks.Select(s => s.ToStockWithoutCommentsDto());

            return new DataResponse<IEnumerable<StockWithoutCommentsDTO>>("Stocks retrieved successfully.", StocksDtos);
        }

        public async Task<DataResponse<StockDto>> GetByIdAsync(int id)
        {
            Stock? stock = await _stockRepository.GetByIdAsync(id);
            if (stock == null)
            {
                return new DataResponse<StockDto>($"Stock with ID {id} not found.");
            }

            return new DataResponse<StockDto>("Stock retrieved successfully.", stock.TostockDto());
        }

        public async Task<DataResponse<StockWithoutCommentsDTO>> CreateAsync(CreateStockRequestDto createStockDto)
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

        public async Task<DataResponse<StockWithoutCommentsDTO>> UpdateAsync(int id, UpdateStockRequestDto stockDto)
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
        
        public async Task<DataResponse<StockWithoutCommentsDTO>> DeleteAsync(int id)
        {
            var stock = await _stockRepository.GetByIdAsync(id);

            if (stock == null)
                return new DataResponse<StockWithoutCommentsDTO>($"Stock with ID {id} not found.");

            var result = await _stockRepository.DeleteAsync(stock);

            if (!result.Succeeded)
                return new DataResponse<StockWithoutCommentsDTO>(
                    result.Errors.FirstOrDefault()?.Description ?? "Failed to delete stock."
                );

            // Agora você pode retornar informações do stock deletado
            return new DataResponse<StockWithoutCommentsDTO>(
                "Stock deleted successfully.",
                stock.ToStockWithoutCommentsDto()
            );
        }
    }
}