using BarberBoss.Domain.Entities;

namespace BarberBoss.Domain.Repositories;
public interface IWriteOnlyRevenueRepository
{
    Task Add(Revenue revenue);
    Task Delete(long id);
}
