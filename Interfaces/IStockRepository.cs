using api.Dtos.Stock;
using api.Helpers;
using api.Models;
using Microsoft.AspNetCore.Identity;

namespace api.Interfaces
{
    public interface IStockRepository
    {
        Task<List<Stock>> GetAllAsync(QueryObject queryObject);

        Task<Stock?> GetByIdAsync(int id);
        Task<IdentityResult> CreateAsync(Stock stock);
        Task<IdentityResult> UpdateAsync(int id, UpdateStockRequestDto stockDto);
        Task<IdentityResult> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id); 
    }
}