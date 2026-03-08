using AutoMapper;
using BarberBoss.Communication.Requests;
using BarberBoss.Domain.Repositories;
using BarberBoss.Domain.Services.LoggedUser;
using BarberBoss.Exception;
using BarberBoss.Exception.ExceptionsBase;

namespace BarberBoss.Application.UseCases.Revenues.Update;
public class UpdateRevenueUseCase : IUpdateRevenueUseCase
{
    private readonly IUpdateOnlyRevenueRepository _repository;
    private readonly IUnitOffWork _unitOffWork;
    private readonly IMapper _mapper;
    private readonly ILoggedUser _loggedUser;

    public UpdateRevenueUseCase(
        IUpdateOnlyRevenueRepository repository, 
        IUnitOffWork unitOffWork, 
        IMapper mapper,
        ILoggedUser loggedUser)
    {
        _mapper = mapper;
        _repository = repository;
        _unitOffWork = unitOffWork;
        _loggedUser = loggedUser;
    }
    public async Task Execute(RequestRevenuesJson request, long id)
    {
        Validate(request);

        var loggedUser = await _loggedUser.Get();

        var revenue = await _repository.GetById(loggedUser, id);

        if(revenue is null)
        {
            throw new NotFoundException(ResourceErrorMessages.REVENUE_NOT_FOUND);
        }

        _mapper.Map(request, revenue);

        _repository.Update(revenue);

        await _unitOffWork.Commit();
    }

    private void Validate(RequestRevenuesJson request)
    {
        var validator = new RevenueUseCaseValidator();
        var result = validator.Validate(request);

        if(result.IsValid is false)
        {
            var errorMessages = result.Errors.Select(errorMessage => errorMessage.ErrorMessage).ToList();

            throw new ErrorsOnValidationException(errorMessages);
        }
    }
}
