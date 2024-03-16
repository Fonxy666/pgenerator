using Microsoft.AspNetCore.Identity;
using PGenerator.Model;

namespace PGenerator.Service.AuthService;

public interface ITokenService
{
    public string CreateJwtToken(IdentityUser user, string? role);
    public RefreshToken CreateRefreshToken();
}