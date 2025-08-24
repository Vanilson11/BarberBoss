using BarberBoss.Domain.Entities;

namespace BarberBoss.Domain.Repositories;
public interface IWriteOnlyRevenueRepository
{
    Task Add(Revenue revenue);

    /// <summary>
    /// The method return TRUE if deletion was successfull. Otherwise, FALSE.
    /// </summary>
    /// <param name="revenue"></param>
    Task<bool> Delete(long id);
}
