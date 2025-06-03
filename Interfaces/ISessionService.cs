using api.Dtos;
using api.Dtos.Account;

namespace api.Interfaces
{
    public interface ISessionService
    {
        Task<DataResponse<TokenDto>> CreateSessionAsync(LoginDto loginDto);
    }
}