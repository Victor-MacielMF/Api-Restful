using api.Dtos;
using api.Dtos.Stock;
using api.Helpers;

namespace api.Interfaces.Services
{
    public interface IAccountStockService
    {
        Task<DataResponse<List<StockDto>>> GetAllAsync(string username, QueryParameters queryObject);

        Task<DataResponse<StockDto>> AddAsync(string username, int stockId);
        
        Task<DataResponse<StockDto>> RemoveAsync(string username, int stockId);
    }
}