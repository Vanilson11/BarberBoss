using BarberBoss.Domain.Entities;

namespace BarberBoss.Domain.Repositories;

public interface IUpdateOnlyUsersRepository
{
    Task<User> GetById(long id);

    void Update(User user);
}
