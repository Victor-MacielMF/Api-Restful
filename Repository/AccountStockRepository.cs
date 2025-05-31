using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class AccountStockRepository : IAccountStockRepository
    {
        private readonly ApplicationDBContext _context;
        public AccountStockRepository(ApplicationDBContext applicationDBContext)
        {
            _context = applicationDBContext;
        }


        public async Task<List<StockDto>> GetStocksDtoByAccountId(Account account)
        {
            if (account == null)
            {
                throw new ArgumentNullException(nameof(account), "Account cannot be null");
            }
            var stocks = LoadStocksAsync(account).Result.Stocks;

            return stocks.Select(s => s.TostockDto()).ToList();
        }

        public async Task<Account> LoadStocksAsync(Account account)
        {
            if (account == null)
            {
                throw new ArgumentNullException(nameof(account), "Account cannot be null");
            }

            var stocks = await _context.Stocks
                .Where(s => s.Accounts.Any(a => a.Id == account.Id))
                //.Include(s => s.Comments) // se quiser trazer comentários também
                .ToListAsync();

            account.Stocks = stocks;
            return account;
            
        }

        public async Task<bool> AddStockToAccountAsync(Account account, int stockId)
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

            var stock = await new StockRepository(_context).GetByIdAsync(stockId);
            if (stock == null)
                throw new ArgumentException("Stock not found", nameof(stockId));

            // Verifica duplicidade
            if (account.Stocks.Any(s => s.Id == stockId))
                return false;

            // Anexa stock, se necessário
            _context.Attach(stock);

            // Faz a associação e salva
            account.Stocks.Add(stock);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RemoveStockFromAccountAsync(Account account, int stock)
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

            var stockToRemove = account.Stocks.FirstOrDefault(s => s.Id == stock);
            if (stockToRemove == null)
                return false;

            // Remove o stock e salva
            account.Stocks.Remove(stockToRemove);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}