using BarberBoss.Communication.Requests;

namespace BarberBoss.Application.UseCases.Users.UpdateProfile;

public interface IUpdateProfileUserUseCase
{
    Task Execute(RequestUpdateUserProfileJson request);
}
