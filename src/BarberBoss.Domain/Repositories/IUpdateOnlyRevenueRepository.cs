using BarberBoss.Domain.Entities;

namespace BarberBoss.Domain.Repositories;
public interface IUpdateOnlyRevenueRepository
{
    Task<Revenue?> GetById(long id);

    void Update(Revenue revenue);
}
