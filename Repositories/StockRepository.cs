using api.Data;
using api.Dtos.Stock;
using api.Helpers;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
    public class StockRepository : IStockRepository
    {

        private readonly ApplicationDBContext _dbContext;

        public StockRepository(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Stock>> GetAllAsync(QueryObject queryObject)
        {
            IQueryable<Stock> stocks = _dbContext.Stocks.AsQueryable();
            if(!string.IsNullOrEmpty(queryObject.Symbol))
            {
                stocks = stocks.Where(s => s.Symbol.Contains(queryObject.Symbol));
            }
            if(!string.IsNullOrEmpty(queryObject.CompanyName))
            {
                stocks = stocks.Where(s => s.CompanyName.Contains(queryObject.CompanyName));
            }
            //Tem que criar o service e fazer uma validação se o campo é valido
            // e que se for igual e tiver tudo em minusculo, fazer ficar igual ao nome da coluna.
            // fazer um if para o comment em espécifico, mas pra isso eu preciso do service.
            if (!string.IsNullOrEmpty(queryObject.SortBy))
            {
                if (queryObject.IsAscending)
                {
                    stocks = stocks.OrderBy(s => EF.Property<object>(s, queryObject.SortBy));
                }
                else
                {
                    stocks = stocks.OrderByDescending(s => EF.Property<object>(s, queryObject.SortBy));
                }
            }
            else
            {
                stocks = queryObject.IsAscending ? stocks.OrderBy(s => s.Id) : stocks.OrderByDescending(s => s.Id); // Default sorting by Id
            }
            int skipNumber = (queryObject.PageNumber - 1 ) * queryObject.PageSize;

            return await stocks.Skip(skipNumber).Take(queryObject.PageSize).ToListAsync();
        }
        public async Task<Stock?> GetByIdAsync(int stockId)
        {
            Stock? stock = await _dbContext.Stocks
                .Where(s => s.Id == stockId)
                .Include(s => s.Comments)
                    .ThenInclude(c => c.Account)
                .FirstOrDefaultAsync();

            if (stock == null)
                return null;

            return stock;
        }

        public async Task<Stock> CreateAsync(Stock stock)
        {
            await _dbContext.Stocks.AddAsync(stock);
            await _dbContext.SaveChangesAsync();

            return stock;
        }

        public async Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto stockDto)
        {
            Stock? existingStock = await _dbContext.Stocks.FirstOrDefaultAsync(s => s.Id == id);
            if (existingStock == null)
            {
                return null;
            }

            existingStock.Symbol = stockDto.Symbol;
            existingStock.CompanyName = stockDto.CompanyName;
            existingStock.Purchase = stockDto.Purchase;
            existingStock.LastDiv = stockDto.LastDiv;
            existingStock.Indutry = stockDto.Indutry;
            existingStock.MarketCap = stockDto.MarketCap;

            await _dbContext.SaveChangesAsync();

            return existingStock;
        }
        
        public async Task<Stock?> DeleteAsync(int id)
        {
            Stock? stock = await GetByIdAsync(id);

            if (stock == null)
            {
                return null;
            }
            _dbContext.Stocks.Remove(stock);
            await _dbContext.SaveChangesAsync();

            return stock;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _dbContext.Stocks.AnyAsync(s => s.Id == id);
        }
    }
}