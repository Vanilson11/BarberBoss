using BarberBoss.Domain.Entities;
using BarberBoss.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BarberBoss.Infrastructure.DataAccess.Repositories;

internal class RevenueRepository : IWriteOnlyRevenueRepository, IReadOnlyRevenueRepository, IUpdateOnlyRevenueRepository, IReportsReadOnlyRepository
{
    private readonly BarberBossDbContext _dbContext;
    public RevenueRepository(BarberBossDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Add(Revenue revenue) => await _dbContext.AddAsync(revenue);

    async Task<Revenue?> IReadOnlyRevenueRepository.GetById(User user, long id) => await _dbContext.Revenues.AsNoTracking().FirstOrDefaultAsync(revenue => revenue.Id == id && revenue.UserId == user.Id);

    public async Task<List<Revenue>> GetAllRevenues(User user) => await _dbContext.Revenues.AsNoTracking().Where(revenue => revenue.UserId == user.Id).ToListAsync();

    async Task<Revenue?> IUpdateOnlyRevenueRepository.GetById(User user,long id) => await _dbContext.Revenues.FirstOrDefaultAsync(revenue => revenue.Id == id && revenue.UserId == user.Id);

    public void Update(Revenue revenue) => _dbContext.Revenues.Update(revenue);

    public async Task Delete(long id)
    {
        var revenue = await _dbContext.Revenues.FirstAsync(revenue => revenue.Id == id);

        _dbContext.Revenues.Remove(revenue);
    }

    public async Task<List<Revenue>> GetByWeek(User user, DateOnly start, DateOnly end)
    {
        var startWeek = new DateTime(start.Year, start.Month, start.Day).Date;
        var endWeek = new DateTime(end.Year, end.Month, end.Day).Date;

        return await _dbContext.Revenues.AsNoTracking()
            .Where(revenue => revenue.UserId == user.Id && revenue.Date >= startWeek && revenue.Date <= endWeek)
            .OrderBy(revenue => revenue.Date)
            .ToListAsync();
    }
}
