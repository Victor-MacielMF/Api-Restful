using api.Dtos;
using api.Dtos.Account;

namespace api.Interfaces.Services
{
    public interface ISessionService
    {
        Task<DataResponse<TokenDto>> CreateSessionAsync(LoginDto loginDto);
    }
}