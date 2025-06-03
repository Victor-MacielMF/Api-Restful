using api.Dtos;
using api.Dtos.Account;
using api.Interfaces;
using api.Models;
using api.Mappers;

namespace api.Services
{
    public class SessionService : ISessionService
    {
        private readonly ISessionRepository _sessionRepository;
        private readonly ITokenService _tokenService;
        public SessionService(ISessionRepository sessionRepository, ITokenService tokenService)
        {
            _sessionRepository = sessionRepository;
            _tokenService = tokenService;
        }

        public async Task<DataResponse<TokenDto>> CreateSessionAsync(LoginDto loginDto)
        {
            Account? account = await _sessionRepository.ValidateUserCredentialsAsync(loginDto.UserName, loginDto.Password);
            if (account == null)
                return new DataResponse<TokenDto>("Invalid username or password.");

            (string token, DateTime expiresAt) = _tokenService.GenerateToken(account);

            TokenDto authTokenDto = token.ToAuthTokenDto(expiresAt);

            return new DataResponse<TokenDto>("Authentication successful.", authTokenDto);
        }
    }
}