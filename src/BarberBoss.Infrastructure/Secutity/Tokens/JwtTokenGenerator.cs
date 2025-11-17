using BarberBoss.Domain.Entities;
using BarberBoss.Domain.Security.Tokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BarberBoss.Infrastructure.Secutity.Tokens;
internal class JwtTokenGenerator : IAccessTokenGenerator
{
    private readonly uint _expirationTimeMinutes;
    private readonly string _signinKey;

    public JwtTokenGenerator(uint expirationTimeMinutes, string signinKey)
    {
        _expirationTimeMinutes = expirationTimeMinutes;
        _signinKey = signinKey;
    }
    public string Generate(User user)
    {
        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Sid, user.UserIdentifier.ToString()),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Expires = DateTime.UtcNow.AddMinutes(_expirationTimeMinutes),
            SigningCredentials = new SigningCredentials(SecurityKey(), SecurityAlgorithms.HmacSha256),
            Subject = new ClaimsIdentity(claims)
        };

        var tokenHandler = new JwtSecurityTokenHandler();//classe para gerar o token
        SecurityToken securityToken = tokenHandler.CreateToken(tokenDescriptor);//cria o token
        
        return tokenHandler.WriteToken(securityToken);
    }

    private SymmetricSecurityKey SecurityKey()
    {
        var key = Encoding.UTF8.GetBytes(_signinKey);

        return new SymmetricSecurityKey(key);
    }
}
