using AutoMapper;
using BarberBoss.Communication.Responses;
using BarberBoss.Domain.Repositories;

namespace BarberBoss.Application.UseCases.Revenues.GetAll;
public class GetAllRevenuesUseCase : IGetAllRevenuesUseCase
{
    private readonly IReadOnlyRevenueRepository _repository;
    private readonly IMapper _mapper;

    public GetAllRevenuesUseCase(IReadOnlyRevenueRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    public async Task<ResponseRevenuesJson> Execute()
    {
        var revenues = await _repository.GetAllRevenues();

        return new ResponseRevenuesJson
        {
            Revenues = _mapper.Map<List<ResponseShortRevenuesJson>>(revenues)
        };
    }
}
