using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace api.Helpers
{
    public static class JwtUtils
    {
        public static SymmetricSecurityKey GetSymmetricSecurityKey(IConfiguration config)
        {
            var secretKey = config["Jwt:SigningKey"];
            if (string.IsNullOrEmpty(secretKey))
            {
                throw new InvalidOperationException("JWT secret key is not configured.");
            }
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        }
    }
}