using api.Data;
using api.Dtos.Stock;
using api.Helpers;
using api.Interfaces;
using api.Interfaces.Repositories;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
    public class AccountStockRepository : IAccountStockRepository
    {
        private readonly ApplicationDBContext _context;
        private readonly IAccountRepository _accountRepository;
        public AccountStockRepository(ApplicationDBContext applicationDBContext,IAccountRepository accountRepository)
        {
            _context = applicationDBContext;
            _accountRepository = accountRepository;
        }


        public async Task<List<Stock>> GetAllByAccountAsync(Account account, QueryParameters queryObject)
        {
            //Se der erro aqui, é porque houve falha na lógica de programação. Então é aceitável lançar uma exceção mesmo que seja no repositório.
            if (account == null)
            {
                throw new ArgumentNullException(nameof(account), "Account cannot be null");
            }
            
            int skipNumber = (queryObject.PageNumber - 1 ) * queryObject.PageSize;

            List<Stock> stocks = await _context.Stocks
                .Where(s => s.Accounts.Any(a => a.Id == account.Id))
                .Include(s => s.Comments)
                    .ThenInclude(c => c.Account)
                .Skip(skipNumber)
                .Take(queryObject.PageSize)
                .ToListAsync();

            return stocks;
        }

        public async Task<IdentityResult> AddAsync(Account account, Stock stock)
        {
            if (account == null)
                return IdentityResult.Failed(new IdentityError { Description = "Account cannot be null." });
            if (stock == null)
                return IdentityResult.Failed(new IdentityError { Description = "Stock cannot be null." });

            // Garante que a navegação está carregada do contexto
                if (!_context.Entry(account).Collection(a => a.Stocks).IsLoaded)
                {
                    Account? loadedAccount = await _accountRepository.GetWithStocksByIdAsync(account.Id);

                    if (loadedAccount == null)
                        return IdentityResult.Failed(new IdentityError { Description = "Account not found." });

                    account = loadedAccount;
                }

            // Verifica duplicidade
            if (account.Stocks.Any(s => s.Id == stock.Id))
                return IdentityResult.Failed(new IdentityError { Description = "Stock already associated with account." });

            _context.Attach(stock);

            account.Stocks.Add(stock);
            await _context.SaveChangesAsync();

            return IdentityResult.Success;
        }

            public async Task<IdentityResult> RemoveAsync(Account account, Stock stockToRemove)
            {
                if (account == null)
                    return IdentityResult.Failed(new IdentityError { Description = "Account cannot be null." });
                if (stockToRemove == null)
                    return IdentityResult.Failed(new IdentityError { Description = "Stock cannot be null." });

                // Garante que a navegação está carregada do contexto
                if (!_context.Entry(account).Collection(a => a.Stocks).IsLoaded)
                {
                    Account? loadedAccount = await _accountRepository.GetWithStocksByIdAsync(account.Id);

                    if (loadedAccount == null)
                        return IdentityResult.Failed(new IdentityError { Description = "Account not found." });

                    account = loadedAccount;
                }

                if (!account.Stocks.Any(s => s.Id == stockToRemove.Id))
                    return IdentityResult.Failed(new IdentityError { Description = "Stock not associated with account." });

                account.Stocks.Remove(stockToRemove);
                await _context.SaveChangesAsync();

                return IdentityResult.Success;
            }

    }
}