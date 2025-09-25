using BarberBoss.Domain.Entities;

namespace BarberBoss.Domain.Repositories;
public interface IWriteOnlyUsersRepository
{
    Task Add(User user);
}
