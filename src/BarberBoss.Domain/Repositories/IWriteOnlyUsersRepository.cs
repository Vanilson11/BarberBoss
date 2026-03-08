using BarberBoss.Domain.Entities;

namespace BarberBoss.Domain.Repositories;
public interface IWriteOnlyUsersRepository
{
    Task Add(User user);

    /// <summary>
    /// Asynchronously deletes the entity with the specified identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the entity to delete.</param>
    /// <returns>A task that represents the asynchronous delete operation. The task result is <see langword="true"/> if the
    /// entity was successfully deleted; otherwise, <see langword="false"/>.</returns>
    Task<bool> Delete(long id);
}
