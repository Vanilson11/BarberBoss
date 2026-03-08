using AutoMapper;
using BarberBoss.Communication.Responses;
using BarberBoss.Domain.Services.LoggedUser;

namespace BarberBoss.Application.UseCases.Users.GetProfile;

public class GetProfileUserUseCase : IGetProfileUserUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IMapper _mapper;

    public GetProfileUserUseCase(ILoggedUser loggedUser, IMapper mapper)
    {
        _loggedUser = loggedUser;
        _mapper = mapper;
    }
    public async Task<ResponseProfileUserJson> Execute()
    {
        var loggedUser = await _loggedUser.Get();

        return _mapper.Map<ResponseProfileUserJson>(loggedUser);
    }
}
