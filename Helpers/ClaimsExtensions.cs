using System.Security.Claims;

namespace api.Extensions
{
    public static class ClaimsExtensions
    {
        public static string GetUsername(this ClaimsPrincipal user)
        {
            var claim = user.Claims
                .SingleOrDefault(x => x.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname"));

            if (claim == null || string.IsNullOrWhiteSpace(claim.Value))
                throw new InvalidOperationException("Username claim is missing.");

            return claim.Value;
        }

        public static string GetAccountId(this ClaimsPrincipal user)
        {
            var claim = user.Claims
                .SingleOrDefault(x => x.Type.Equals("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"));

            if (claim == null || string.IsNullOrWhiteSpace(claim.Value))
                throw new InvalidOperationException("Account ID claim is missing.");

            return claim.Value;
        }
    }
}