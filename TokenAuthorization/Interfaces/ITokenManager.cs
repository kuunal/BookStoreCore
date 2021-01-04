using ModelLayer;
using System.Security.Claims;

namespace TokenAuthentication
{
    public interface ITokenManager
    {
        string Encode(UserDto account, int JwtExpiryTime, byte[] secret);
        string Encode(UserDto account);

        ClaimsPrincipal Decode(string token);
        ClaimsPrincipal Decode(string token, byte[] secret);
    }
}
