using BarberBoss.Domain.Entities;

namespace BarberBoss.Domain.Repositories;
public interface IReadOnlyUsersRepository
{
    Task<User?> GetById(long id);
    Task<bool> EmailExists(string email);
    Task<User?> GetUserByEmail(string email);
}
