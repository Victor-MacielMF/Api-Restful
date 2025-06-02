using api.Dtos;
using api.Dtos.Account;

namespace api.Interfaces
{
    public interface IAccountService
    {
        public Task<DataResponse<AccountDto>> RegisterAsync(CreateAccountDto dto);
    }
}