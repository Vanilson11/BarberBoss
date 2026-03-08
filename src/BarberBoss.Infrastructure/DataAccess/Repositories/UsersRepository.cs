using BarberBoss.Domain.Entities;
using BarberBoss.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BarberBoss.Infrastructure.DataAccess.Repositories;
internal class UsersRepository : IReadOnlyUsersRepository, IWriteOnlyUsersRepository, IUpdateOnlyUsersRepository
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

    async Task<User> IUpdateOnlyUsersRepository.GetById(long id)
    {
        return await _dbContext.Users.FirstAsync(user => user.Id == id);
    }

    public async Task<User?> GetUserByEmail(string email) {
        return await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Email.Equals(email));
    }

    public void Update(User user)
    {
        _dbContext.Users.Update(user);
    }

    async Task<User?> IReadOnlyUsersRepository.GetById(long id)
    {
        return await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Id == id);
    }

    public async Task<bool> Delete(long id)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(user => user.Id == id);

        if (user == null)
        {
            return false;
        }

        _dbContext.Users.Remove(user);

        return true;
    }
}
