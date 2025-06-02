using api.Dtos;
using api.Dtos.Account;
using api.Interfaces;
using api.Mappers;

namespace api.Services
{
    public class AccountService : IAccountService
    {   
        private readonly IAccountRepository _accountRepository;
        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<DataResponse<AccountDto>> RegisterAsync(CreateAccountDto dto)
        {
            var account = dto.ToAccount();
            
            var result = await _accountRepository.CreateAsync(account, dto.Password);

            if (result.Succeeded)
            {
                var accountDto = account.ToAccountDto();
                return new DataResponse<AccountDto>("Account created successfully.", accountDto);
            }
            else
            {
                return new DataResponse<AccountDto>("Failed to create account.", result.Errors.Select(e => e.Description));
            }
        }   
    }
}