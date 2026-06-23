using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PolicySearchService.Models.Authorization;

namespace PolicySearchService.Services.AuthService;

public class AuthService : IAuthService
{
    private readonly JwtSettings _jwt;
    private readonly AuthSettings _auth;

    public AuthService(IOptions<JwtSettings> jwtOptions, IOptions<AuthSettings> authOptions)
    {
        _jwt = jwtOptions.Value;
        _auth = authOptions.Value;
    }

    public AuthResponse? Authenticate(AuthRequest request)
    {
        if (request.Login != _auth.Login ||
            request.Password != _auth.Password ||
            request.OrgCode != _auth.OrgCode)
            return null;

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, request.Login),
            new Claim("orgCode", request.OrgCode),
            new Claim("login", request.Login)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwt.Issuer,
            audience: _jwt.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwt.ExpireMinutes),
            signingCredentials: creds);

        return new AuthResponse { Token = new JwtSecurityTokenHandler().WriteToken(token) };
    }
}
