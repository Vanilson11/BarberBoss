using BarberBoss.Domain.Entities;

namespace BarberBoss.Domain.Repositories;
public interface IReadOnlyRevenueRepository
{
    Task<List<Revenue>> GetAllRevenues();

    Task<Revenue?> GetById(long id);
}
