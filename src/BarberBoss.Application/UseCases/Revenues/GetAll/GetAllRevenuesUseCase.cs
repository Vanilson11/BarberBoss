using AutoMapper;
using BarberBoss.Communication.Responses;
using BarberBoss.Domain.Repositories;
using BarberBoss.Domain.Services.LoggedUser;

namespace BarberBoss.Application.UseCases.Revenues.GetAll;
public class GetAllRevenuesUseCase : IGetAllRevenuesUseCase
{
    private readonly IReadOnlyRevenueRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILoggedUser _loggedUser;

    public GetAllRevenuesUseCase(IReadOnlyRevenueRepository repository, IMapper mapper, ILoggedUser loggedUser)
    {
        _repository = repository;
        _mapper = mapper;
        _loggedUser = loggedUser;
    }
    public async Task<ResponseRevenuesJson> Execute()
    {
        var loggedUser = await _loggedUser.Get();

        var revenues = await _repository.GetAllRevenues(loggedUser);

        return new ResponseRevenuesJson
        {
            Revenues = _mapper.Map<List<ResponseShortRevenuesJson>>(revenues)
        };
    }
}
