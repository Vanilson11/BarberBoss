using AutoMapper;
using BarberBoss.Communication.Responses;
using BarberBoss.Domain.Repositories;
using BarberBoss.Exception;
using BarberBoss.Exception.ExceptionsBase;

namespace BarberBoss.Application.UseCases.Users.GetById;

public class GetUserByIdUseCase : IGetUserByIdUseCase
{
    private readonly IReadOnlyUsersRepository _readOnlyUsersRepository;
    private readonly IMapper _mapper;

    public GetUserByIdUseCase(IReadOnlyUsersRepository readOnlyUsersRepository, IMapper mapper)
    {
        _readOnlyUsersRepository = readOnlyUsersRepository;
        _mapper = mapper;
    }
    public async Task<ResponseProfileUserJson> Execute(long id)
    {
        var user = await _readOnlyUsersRepository.GetById(id);

        if (user is null) throw new NotFoundException(ResourceErrorMessages.USER_NOT_FOUND);

        return _mapper.Map<ResponseProfileUserJson>(user);
    }
}
