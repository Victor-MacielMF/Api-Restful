using api.Models;
using Microsoft.AspNetCore.Identity;

namespace api.Interfaces.Repositories
{
    public interface IAccountStockRepository
    {
        Task<List<Stock>> GetAllByAccountAsync(Account accountId);
        Task<IdentityResult> AddAsync(Account account, Stock stock);
        Task<IdentityResult> RemoveAsync(Account account, Stock stock);
    }
}