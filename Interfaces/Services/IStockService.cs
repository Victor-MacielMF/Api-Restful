using api.Dtos;
using api.Dtos.Stock;
using api.Helpers;

namespace api.Interfaces.Services
{
    public interface IStockService
    {
        Task<DataResponse<IEnumerable<StockWithoutCommentsDTO>>> GetAllAsync(QueryObject queryObject);
        Task<DataResponse<StockDto>> GetByIdAsync(int id);
        Task<DataResponse<StockWithoutCommentsDTO>> CreateAsync(CreateStockRequestDto stockDto);
        Task<DataResponse<StockWithoutCommentsDTO>> UpdateAsync(int id, UpdateStockRequestDto stockDto);
        Task<DataResponse<StockWithoutCommentsDTO>> DeleteAsync(int id);
    }
}