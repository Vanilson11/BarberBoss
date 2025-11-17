using BarberBoss.Communication.Requests;
using BarberBoss.Communication.Responses;

namespace BarberBoss.Application.UseCases.DoLogin;
public interface IDoLoginUseCase
{
    Task<ResponseRegisterUserJson> Execute(RequestDoLoginJson request);
}
