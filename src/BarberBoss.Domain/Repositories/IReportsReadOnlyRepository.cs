using BarberBoss.Domain.Entities;

namespace BarberBoss.Domain.Repositories;
public interface IReportsReadOnlyRepository
{
    Task<List<Revenue>> GetByWeek(DateOnly start, DateOnly end);
}
