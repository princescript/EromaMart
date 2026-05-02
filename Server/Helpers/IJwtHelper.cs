using Microsoft.IdentityModel.Tokens;
using Server.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace Server.Helpers;

public interface IJwtHelper
{
    string GenerateToken(UserMaster user);
}
public class JwtHelper : IJwtHelper
{
    private readonly IConfiguration _configuration;
    public JwtHelper(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public string GenerateToken(UserMaster user)
    {
        var jwtSettings = _configuration.GetSection("Jwt");
        var secretKey = jwtSettings.GetValue<string>("Secret")
                ?? throw new InvalidOperationException("JWT Secret is not configured");
        var issuer = jwtSettings.GetValue<string>("Issuer")
                     ?? throw new InvalidOperationException("JWT Issuer is not configured");
        var audience = jwtSettings.GetValue<string>("Audience")
                       ?? throw new InvalidOperationException("JWT Audience is not configured");
        var expiryHours = jwtSettings.GetValue<int>("ExpiryHours");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.user_id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.email!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(expiryHours),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

}
