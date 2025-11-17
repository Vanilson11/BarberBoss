using BarberBoss.Domain.Entities;

namespace BarberBoss.Domain.Repositories;
public interface IReportsReadOnlyRepository
{
    Task<List<Revenue>> GetByWeek(User user, DateOnly start, DateOnly end);
}
