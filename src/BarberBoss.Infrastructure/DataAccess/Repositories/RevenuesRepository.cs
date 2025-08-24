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

    async Task<Revenue?> IReadOnlyRevenueRepository.GetById(long id) => await _dbContext.Revenues.AsNoTracking().FirstOrDefaultAsync(revenue => revenue.Id == id);

    public async Task<List<Revenue>> GetAllRevenues() => await _dbContext.Revenues.AsNoTracking().ToListAsync();

    async Task<Revenue?> IUpdateOnlyRevenueRepository.GetById(long id) => await _dbContext.Revenues.FirstOrDefaultAsync(revenue =>revenue.Id == id);

    public void Update(Revenue revenue) => _dbContext.Revenues.Update(revenue);

    public async Task<bool> Delete(long id)
    {
        var revenue = await _dbContext.Revenues.FirstOrDefaultAsync(revenue => revenue.Id == id);

        if (revenue is null)
            return false;

        _dbContext.Revenues.Remove(revenue);

        return true;
    }

    public async Task<List<Revenue>> GetByWeek(DateOnly start, DateOnly end)
    {
        var startWeek = new DateTime(start.Year, start.Month, start.Day).Date;
        var endWeek = new DateTime(end.Year, end.Month, end.Day).Date;

        return await _dbContext.Revenues.AsNoTracking()
            .Where(revenue => revenue.Date >= startWeek && revenue.Date <= endWeek)
            .OrderBy(revenue => revenue.Date)
            .ToListAsync();
    }
}
