using AutoMapper;
using BarberBoss.Communication.Requests;
using BarberBoss.Communication.Responses;
using BarberBoss.Domain.Entities;
using BarberBoss.Domain.Repositories;
using BarberBoss.Exception;
using BarberBoss.Exception.ExceptionsBase;

namespace BarberBoss.Application.UseCases.Revenues.Update;
public class UpdateRevenueUseCase : IUpdateRevenueUseCase
{
    private readonly IUpdateOnlyRevenueRepository _repository;
    private readonly IUnitOffWork _unitOffWork;
    private readonly IMapper _mapper;

    public UpdateRevenueUseCase(IUpdateOnlyRevenueRepository repository, IUnitOffWork unitOffWork, IMapper mapper)
    {
        _mapper = mapper;
        _repository = repository;
        _unitOffWork = unitOffWork;
    }
    public async Task Execute(RequestRevenuesJson request, long id)
    {
        Validate(request);

        var result = await _repository.GetById(id);

        if(result is null)
        {
            throw new NotFoundException(ResourceErrorMessages.REVENUE_NOT_FOUND);
        }

        _mapper.Map(request, result);

        _repository.Update(result);

        await _unitOffWork.Commit();
    }

    private void Validate(RequestRevenuesJson request)
    {
        var validator = new RevenueUseCaseValidator();
        var result = validator.Validate(request);

        if(result.IsValid is false)
        {
            var errorMessages = result.Errors.Select(errorMessage => errorMessage.ErrorMessage).ToList();
            var responseErrorMessages = new ResponseErrorMessagesJson(errorMessages);

            throw new ErrorsOnValidationException(errorMessages);
        }
    }
}
