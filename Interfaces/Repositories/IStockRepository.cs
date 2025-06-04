using api.Dtos.Stock;
using api.Helpers;
using api.Models;
using Microsoft.AspNetCore.Identity;

namespace api.Interfaces.Repositories
{
    public interface IStockRepository
    {
        Task<List<Stock>> GetAllAsync(StockQueryParameters queryObject);

        Task<Stock?> GetByIdAsync(int id);
        Task<IdentityResult> CreateAsync(Stock stock);
        Task<IdentityResult> UpdateAsync(int id, UpdateStockRequestDto stockDto);
        Task<IdentityResult> DeleteAsync(Stock stock);
        Task<bool> ExistsAsync(int id); 
    }
}