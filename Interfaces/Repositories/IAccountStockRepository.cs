using api.Models;
using Microsoft.AspNetCore.Identity;

namespace api.Interfaces.Repositories
{
    public interface IAccountStockRepository
    {
        Task<List<Stock>> GetStocksDtoByAccountId(Account accountId);
        Task<IdentityResult> AddStockToAccountAsync(Account account, Stock stock);
        Task<IdentityResult> RemoveStockFromAccountAsync(Account account, Stock stock);
    }
}