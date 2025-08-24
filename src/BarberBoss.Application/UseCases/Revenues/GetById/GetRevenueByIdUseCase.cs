using AutoMapper;
using BarberBoss.Communication.Responses;
using BarberBoss.Domain.Repositories;
using BarberBoss.Exception;
using BarberBoss.Exception.ExceptionsBase;

namespace BarberBoss.Application.UseCases.Revenues.GetById;
public class GetRevenueByIdUseCase : IGetRevenueByIdUseCase
{
    private readonly IReadOnlyRevenueRepository _repository;
    private readonly IMapper _mapper;

    public GetRevenueByIdUseCase(IReadOnlyRevenueRepository repository, IMapper mapper)
    {
        _mapper = mapper;
        _repository = repository;
    }
    public async Task<ResponseRevenueJson> Execute(long id)
    {
        var revenue = await _repository.GetById(id);

        if(revenue is null)
        {
            throw new NotFoundException(ResourceErrorMessages.REVENUE_NOT_FOUND);
        }

        return _mapper.Map<ResponseRevenueJson>(revenue);
    }
}
