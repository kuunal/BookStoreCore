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

        public string Encode(UserResponseDto account)
        {
            return Encode(account, expiryTime);
        }

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

        public ClaimsPrincipal Decode(string token)
        {
            return Decode(token, secretKey);
        }


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
