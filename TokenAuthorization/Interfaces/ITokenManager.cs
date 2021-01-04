using ModelLayer;
using System.Security.Claims;

namespace TokenAuthentication
{
    public interface ITokenManager
    {
        string Encode(UserResponseDto account, int JwtExpiryTime, byte[] secret);
        string Encode(UserResponseDto account);

        ClaimsPrincipal Decode(string token);
        ClaimsPrincipal Decode(string token, byte[] secret);
    }
}
