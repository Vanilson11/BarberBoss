using BarberBoss.Communication.Responses;

namespace BarberBoss.Application.UseCases.Users.GetProfile;

public interface IGetProfileUserUseCase
{
    Task<ResponseProfileUserJson> Execute();
}
