using api.Dtos;
using api.Dtos.Stock;

namespace api.Interfaces.Services
{
    public interface IAccountStockService
    {
        Task<DataResponse<List<StockDto>>> GetStocksByAccountAsync(string username);

        Task<DataResponse<StockDto>> AddStockToAccountAsync(string username, int stockId);
        
        Task<DataResponse<StockDto>> RemoveStockFromAccountAsync(string username, int stockId);
    }
}