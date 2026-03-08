namespace BarberBoss.Application.UseCases.Users.Delete;

public interface IDeleteUserUseCase
{
    Task Execute(long id);
}
