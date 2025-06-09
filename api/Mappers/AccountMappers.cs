using api.Dtos.Account;
using api.Models;

namespace api.Mappers
{
    public static class AccountMappers
    {
        public static Account ToAccount(this CreateAccountDto accountDto)
        {
            return new Account
            {
                UserName = accountDto.UserName,
                Email = accountDto.Email
                // Additional properties can be mapped here as needed
            };
        }

        public static AccountDto ToAccountDto(this Account account)
        {
            return new AccountDto
            {
                UserName = account.UserName,
                Email = account.Email
                // Additional properties can be mapped here as needed
            };
        }

        public static TokenDto ToAuthTokenDto(this string token,DateTime expiration)
        {
            return new TokenDto
            {
                Token = token,
                ExpiresAt = expiration,
            };
        }

    }
}