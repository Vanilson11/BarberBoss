using BarberBoss.Domain.Entities;
using BarberBoss.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BarberBoss.Infrastructure.DataAccess.Repositories;
internal class UsersRepository : IReadOnlyUsersRepository, IWriteOnlyUsersRepository
{
    private readonly BarberBossDbContext _dbContext;

    public UsersRepository(BarberBossDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Add(User user)
    {
        await _dbContext.Users.AddAsync(user);
    }

    public async Task<bool> EmailExists(string email)
    {
        return await _dbContext.Users.AsNoTracking().AnyAsync(user => user.Email.Equals(email));
    }
}
