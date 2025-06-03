using api.Dtos;
using api.Dtos.Stock;

namespace api.Interfaces.Services
{
    public interface IAccountStockService
    {
        Task<DataResponse<List<StockDto>>> GetAllAsync(string username);

        Task<DataResponse<StockDto>> AddAsync(string username, int stockId);
        
        Task<DataResponse<StockDto>> RemoveAsync(string username, int stockId);
    }
}