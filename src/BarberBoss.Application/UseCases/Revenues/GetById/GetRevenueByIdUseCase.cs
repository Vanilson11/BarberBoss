using AutoMapper;
using BarberBoss.Communication.Responses;
using BarberBoss.Domain.Repositories;
using BarberBoss.Domain.Services.LoggedUser;
using BarberBoss.Exception;
using BarberBoss.Exception.ExceptionsBase;

namespace BarberBoss.Application.UseCases.Revenues.GetById;
public class GetRevenueByIdUseCase : IGetRevenueByIdUseCase
{
    private readonly IReadOnlyRevenueRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILoggedUser _loggedUser;

    public GetRevenueByIdUseCase(IReadOnlyRevenueRepository repository, IMapper mapper, ILoggedUser loggedUser)
    {
        _mapper = mapper;
        _repository = repository;
        _loggedUser = loggedUser;
    }
    public async Task<ResponseRevenueJson> Execute(long id)
    {
        var loggedUser = await _loggedUser.Get();

        var revenue = await _repository.GetById(loggedUser, id);

        if(revenue is null)
        {
            throw new NotFoundException(ResourceErrorMessages.REVENUE_NOT_FOUND);
        }

        return _mapper.Map<ResponseRevenueJson>(revenue);
    }
}
