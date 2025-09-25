namespace BarberBoss.Domain.Repositories;
public interface IReadOnlyUsersRepository
{
    Task<bool> EmailExists(string email);
}
