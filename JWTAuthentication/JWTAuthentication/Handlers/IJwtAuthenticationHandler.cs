using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace JWTAuthentication.Handlers
{
    public interface IJwtAuthenticationHandler
    {
        JwtSecurityToken GetToken(List<Claim> authClaims);
    }
}
