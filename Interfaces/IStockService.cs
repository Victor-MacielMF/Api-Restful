using api.Dtos;
using api.Dtos.Stock;
using api.Helpers;
using Microsoft.AspNetCore.Identity;

namespace api.Interfaces
{
    public interface IStockService
    {
        Task<DataResponse<IEnumerable<StockWithoutCommentsDTO>>> GetStocks(QueryObject queryObject);
        Task<DataResponse<StockDto>> GetStock(int id);
        Task<DataResponse<StockWithoutCommentsDTO>> PostStock(CreateStockRequestDto stockDto);
        Task<DataResponse<StockWithoutCommentsDTO>> PutStock(int id, UpdateStockRequestDto stockDto);
        Task<DataResponse<StockWithoutCommentsDTO>> DeleteStockAsync(int id);
    }
}