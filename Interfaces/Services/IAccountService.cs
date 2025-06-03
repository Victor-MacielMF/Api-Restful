using api.Dtos;
using api.Dtos.Account;

namespace api.Interfaces.Services
{
    public interface IAccountService
    {
        public Task<DataResponse<AccountDto>> RegisterAsync(CreateAccountDto dto);
    }
}