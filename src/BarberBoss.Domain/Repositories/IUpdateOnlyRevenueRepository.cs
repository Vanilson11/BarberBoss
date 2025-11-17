using BarberBoss.Domain.Entities;

namespace BarberBoss.Domain.Repositories;
public interface IUpdateOnlyRevenueRepository
{
    Task<Revenue?> GetById(User user, long id);

    void Update(Revenue revenue);
}
