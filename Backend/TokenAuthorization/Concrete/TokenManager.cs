using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ModelLayer;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TokenAuthorization;

namespace Greeting.TokenAuthorization
{
    public class TokenManager : ITokenManager
    {
        private readonly JwtSecurityTokenHandler tokenHandler;
        private readonly byte[] secretKey;
        private readonly int expiryTime;

        public TokenManager(IConfiguration configuration)
        {
            tokenHandler = new JwtSecurityTokenHandler();
            secretKey = Encoding.ASCII.GetBytes(configuration.GetSection("Jwt")["AccessSecretKey"]);
            expiryTime = Convert.ToInt32(configuration.GetSection("Jwt")["ExpiryTime"]);

        }

        /// <summary>
        /// Generates jwt with default expiry time.
        /// </summary>
        /// <param name="account">The account.</param>
        /// <returns>Token</returns>
        public string Encode(UserResponseDto account)
        {
            return Encode(account, expiryTime);
        }

        /// <summary>
        /// Generates jwt with custom parameters.
        /// </summary>
        /// <param name="account">The account.</param>
        /// <param name="ExpiryTimeInMinutes">The expiry time in minutes.</param>
        /// <param name="secret">The secret.</param>
        /// <returns>Token</returns>
        public string Encode(UserResponseDto account, int ExpiryTimeInMinutes, byte[] secret=null)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] { 
                    new Claim(ClaimTypes.UserData, account.Id.ToString()),
                    new Claim(ClaimTypes.Email, account.Email),
                    new Claim(ClaimTypes.Role, account.Role)
                }),
                Expires = DateTime.UtcNow.AddMinutes(ExpiryTimeInMinutes),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(secret ?? secretKey),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        /// <summary>
        /// Decodes the with default token.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        public ClaimsPrincipal Decode(string token)
        {
            return Decode(token, secretKey);
        }

        /// <summary>
        /// Decodes with specified token.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="secret">The secret.</param>
        /// <returns></returns>
        public ClaimsPrincipal Decode(string token, byte[] secret)
        {
            var claims = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(secret),
                ValidateLifetime = true,
                ValidateAudience = false,
                ValidateIssuer = false,
                ClockSkew = TimeSpan.FromMinutes(0)
            }, out SecurityToken validatedToken);
            return claims;
        }
    }
}
