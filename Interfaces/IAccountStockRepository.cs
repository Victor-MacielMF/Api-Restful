using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Stock;
using api.Models;

namespace api.Interfaces
{
    public interface IAccountStockRepository
    {
        Task<List<StockDto>> GetStocksDtoByAccountId(Account accountId);
        Task<Account> LoadStocksAsync(Account account);
        Task<bool> AddStockToAccountAsync(Account account, int stock);
        Task<bool> RemoveStockFromAccountAsync(Account account, int stock);
    }
}