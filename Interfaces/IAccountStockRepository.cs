using api.Dtos.Stock;
using api.Models;

namespace api.Interfaces
{
    public interface IAccountStockRepository
    {
        Task<List<StockDto>> GetStocksDtoByAccountId(Account accountId);
        Task<Stock> AddStockToAccountAsync(Account account, int stock);
        Task<Stock> RemoveStockFromAccountAsync(Account account, int stock);
    }
}