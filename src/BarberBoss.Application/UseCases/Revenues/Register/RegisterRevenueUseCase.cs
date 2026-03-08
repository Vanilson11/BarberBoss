using AutoMapper;
using BarberBoss.Communication.Requests;
using BarberBoss.Communication.Responses;
using BarberBoss.Domain.Entities;
using BarberBoss.Domain.Repositories;
using BarberBoss.Domain.Services.LoggedUser;
using BarberBoss.Exception.ExceptionsBase;

namespace BarberBoss.Application.UseCases.Revenues.Register;
public class RegisterRevenueUseCase : IRegisterRevenueUseCase
{
    private readonly IWriteOnlyRevenueRepository _repository;
    private readonly IMapper _mapper;
    private readonly IUnitOffWork _unitOffWork;
    private readonly ILoggedUser _loggedUser;

    public RegisterRevenueUseCase(
        IWriteOnlyRevenueRepository repository, 
        IUnitOffWork unitOffWork, 
        IMapper mapper,
        ILoggedUser loggedUser)
    {
        _repository = repository;
        _unitOffWork = unitOffWork;
        _mapper = mapper;
        _loggedUser = loggedUser;
    }
    public async Task<ResponseRegisterRevenueJson> Execute(RequestRevenuesJson request)
    {
        Validate(request);

        var loggedUser = await _loggedUser.Get();

        var revenue = _mapper.Map<Revenue>(request);

        revenue.UserId = loggedUser.Id;

        await _repository.Add(revenue);

        await _unitOffWork.Commit();

        return _mapper.Map<ResponseRegisterRevenueJson>(revenue);
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
