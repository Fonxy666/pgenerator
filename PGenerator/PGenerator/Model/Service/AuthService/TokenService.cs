using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace PGenerator.Model.Service.AuthService;

public class TokenService(IConfiguration configuration) : ITokenService
{
    private const int ExpirationHours = 2;
    
    public string CreateJwtToken(IdentityUser user, string? role)
    {
        var expiration = DateTime.UtcNow.AddHours(ExpirationHours);
        var token = CreateJwtToken(CreateClaims(user, role), CreateSigningCredentials(), expiration);

        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(token);
    }

    public RefreshToken CreateRefreshToken()
    {
        return new RefreshToken
        {
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            Expires = DateTime.UtcNow.AddDays(7)
        };
    }
    
    private JwtSecurityToken CreateJwtToken(List<Claim> claims, SigningCredentials credentials, DateTime expiration)
    {
        return new JwtSecurityToken(
            issuer: configuration["IssueAudience"],
            audience: configuration["IssueAudience"],
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: expiration,
            signingCredentials: credentials
        );
    }
    
    public List<Claim> CreateClaims(IdentityUser user, string? role)
    {
        try
        {
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, configuration["IssueAudience"]!),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)),
                new(ClaimTypes.NameIdentifier, user.Id)
            };

            if (!string.IsNullOrEmpty(user.UserName))
            {
                claims.Add(new(ClaimTypes.Name, user.UserName));
            }

            if (!string.IsNullOrEmpty(user.Email))
            {
                claims.Add(new(ClaimTypes.Email, user.Email));
            }

            if (role != null)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private SigningCredentials CreateSigningCredentials()
    {
        return new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["IssueSign"]!)), SecurityAlgorithms.HmacSha256);
    }
}