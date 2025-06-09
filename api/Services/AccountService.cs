using api.Dtos;
using api.Dtos.Account;
using api.Interfaces.Repositories;
using api.Interfaces.Services;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Identity;

namespace api.Services
{
    public class AccountService : IAccountService
    {   
        private readonly IAccountRepository _accountRepository;
        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<DataResponse<AccountDto>> CreateAsync(CreateAccountDto dto)
        {
            Account account = dto.ToAccount();
            
            IdentityResult result = await _accountRepository.CreateAsync(account, dto.Password);

            if (result.Succeeded)
            {
                AccountDto accountDto = account.ToAccountDto();
                return new DataResponse<AccountDto>("Account created successfully.", accountDto);
            }
            else
            {
                return new DataResponse<AccountDto>("Failed to create account.", result.Errors.Select(e => e.Description));
            }
        }   
    }
}