using api.Data;
using api.Dtos.Stock;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
    public class AccountStockRepository : IAccountStockRepository
    {
        private readonly ApplicationDBContext _context;
        private readonly IStockRepository _stockRepository;
        public AccountStockRepository(ApplicationDBContext applicationDBContext, IStockRepository stockRepository)
        {
            _context = applicationDBContext;
            _stockRepository = stockRepository;
        }


        public async Task<List<StockDto>> GetStocksDtoByAccountId(Account account)
        {
            if (account == null)
            {
                throw new ArgumentNullException(nameof(account), "Account cannot be null");
            }
            //var stocks = (await LoadStocksAsync(account)).Stocks;
            var stocks = await _context.Stocks
                .Where(s => s.Accounts.Any(a => a.Id == account.Id))
                .Include(s => s.Comments)
                    .ThenInclude(c => c.Account)
                .ToListAsync();
            account.Stocks = stocks;

            return stocks.Select(s => s.TostockDto(account.Id)).ToList();
        }

        public async Task<Stock> AddStockToAccountAsync(Account account, int stockId)
        {
            if (account == null)
                throw new ArgumentNullException(nameof(account));

            if (!_context.Entry(account).Collection(a => a.Stocks).IsLoaded){
                // Garante que a navegação está carregada do contexto
                var loadedAccount = await _context.Users
                    .Include(a => a.Stocks)
                    .FirstOrDefaultAsync(a => a.Id == account.Id);

                if (loadedAccount == null)
                    throw new InvalidOperationException("Account not found.");

                account = loadedAccount;
            }

            var stock = await _stockRepository.GetByIdAsync(stockId);
            if (stock == null)
                throw new ArgumentException("Stock not found", nameof(stockId));

            // Verifica duplicidade
            if (account.Stocks.Any(s => s.Id == stockId))
                return null;

            // Anexa stock, se necessário
            _context.Attach(stock);

            // Faz a associação e salva
            account.Stocks.Add(stock);
            await _context.SaveChangesAsync();

            return stock;
        }

        public async Task<Stock> RemoveStockFromAccountAsync(Account account, int stock)
        {
            if (account == null)
                throw new ArgumentNullException(nameof(account));

            if (!_context.Entry(account).Collection(a => a.Stocks).IsLoaded){
                // Garante que a navegação está carregada do contexto
                var loadedAccount = await _context.Users
                    .Include(a => a.Stocks)
                    .FirstOrDefaultAsync(a => a.Id == account.Id);

                if (loadedAccount == null)
                    throw new InvalidOperationException("Account not found.");

                account = loadedAccount;
            }

            var stockToRemove = await _stockRepository.GetByIdAsync(stock);
            if (stockToRemove == null)
                return null;

            // Remove o stock e salva
            account.Stocks.Remove(stockToRemove);
            await _context.SaveChangesAsync();

            return stockToRemove;
        }
    }
}