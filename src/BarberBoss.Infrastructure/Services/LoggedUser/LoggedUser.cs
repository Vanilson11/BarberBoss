using BarberBoss.Domain.Entities;
using BarberBoss.Domain.Security.Tokens;
using BarberBoss.Domain.Services.LoggedUser;
using BarberBoss.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BarberBoss.Infrastructure.Services.LoggedUser;
internal class LoggedUser : ILoggedUser
{
    private readonly ITokenProvider _tokenProvider;
    private readonly BarberBossDbContext _dbContext;

    public LoggedUser(ITokenProvider tokenProvider, BarberBossDbContext dbContext)
    {
        _tokenProvider = tokenProvider;
        _dbContext = dbContext;
    }
    public async Task<User> Get()
    {
        var token = _tokenProvider.TokenOnRequest();
        var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        var jwtSecurityToken = jwtSecurityTokenHandler.ReadJwtToken(token);
        var identifier = jwtSecurityToken.Claims.First(claim => claim.Type == ClaimTypes.Sid).Value;

        return await _dbContext.Users.AsNoTracking().FirstAsync(user => user.UserIdentifier == Guid.Parse(identifier));
    }
}
