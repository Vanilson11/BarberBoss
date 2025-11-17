using BarberBoss.Domain.Entities;

namespace BarberBoss.Domain.Repositories;
public interface IReadOnlyRevenueRepository
{
    Task<List<Revenue>> GetAllRevenues(User user);

    Task<Revenue?> GetById(User user, long id);
}
