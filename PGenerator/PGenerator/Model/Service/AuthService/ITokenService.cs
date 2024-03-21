using Microsoft.AspNetCore.Identity;

namespace PGenerator.Model.Service.AuthService;

public interface ITokenService
{
    public string CreateJwtToken(IdentityUser user, string? role);
    public RefreshToken CreateRefreshToken();
}