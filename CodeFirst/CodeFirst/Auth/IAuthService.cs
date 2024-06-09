using System.Security.Claims;

namespace CodeFirst.Auth;

public interface IAuthService
{
    string GenerateAccessToken(IEnumerable<Claim> claims);
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}