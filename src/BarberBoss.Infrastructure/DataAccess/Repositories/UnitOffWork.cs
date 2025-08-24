using BarberBoss.Domain.Repositories;

namespace BarberBoss.Infrastructure.DataAccess.Repositories;
internal class UnitOffWork : IUnitOffWork
{
    private readonly BarberBossDbContext _dbContext;
    public UnitOffWork(BarberBossDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task Commit() => await _dbContext.SaveChangesAsync();
}
