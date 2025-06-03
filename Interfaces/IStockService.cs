using api.Dtos;
using api.Dtos.Stock;
using api.Helpers;

namespace api.Interfaces
{
    public interface IStockService
    {
        Task<DataResponse<IEnumerable<StockWithoutCommentsDTO>>> GetStocks(QueryObject queryObject);
        Task<DataResponse<StockDto>> GetStock(int id);
    }
}