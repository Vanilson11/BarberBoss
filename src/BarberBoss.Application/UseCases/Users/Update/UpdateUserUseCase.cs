using AutoMapper;
using BarberBoss.Communication.Requests;
using BarberBoss.Domain.Repositories;
using BarberBoss.Exception;
using BarberBoss.Exception.ExceptionsBase;
using FluentValidation.Results;

namespace BarberBoss.Application.UseCases.Users.Update;

public class UpdateUserUseCase : IUpdateUserUseCase
{
    private readonly IUpdateOnlyUsersRepository _repository;
    private readonly IReadOnlyUsersRepository _readOnlyUsersRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOffWork _unitOfWork;

    public UpdateUserUseCase(
        IUpdateOnlyUsersRepository updateOnlyUsersRepository,
        IReadOnlyUsersRepository readOnlyUsersRepository,
        IMapper mapper,
        IUnitOffWork unitOfWork
        )
    {
        _repository = updateOnlyUsersRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _readOnlyUsersRepository = readOnlyUsersRepository;
    }
    public async Task Execute(long id, RequestUpdateUserJson request)
    {
        var user = await _readOnlyUsersRepository.GetById(id);

        if(user is null)
        {
            throw new NotFoundException(ResourceErrorMessages.USER_NOT_FOUND);
        }

        await Validate(request, user.Email);

        _mapper.Map(request, user);

        _repository.Update(user);

        await _unitOfWork.Commit();
    }

    private async Task Validate(RequestUpdateUserJson request, string currentEmail)
    {
        var result = new UpdateUserValidator().Validate(request);

        if(currentEmail.Equals(request.Email) == false)
        {
            var emailExists = await _readOnlyUsersRepository.EmailExists(request.Email);

            if (emailExists)
            {
                result.Errors.Add(new ValidationFailure(string.Empty, ResourceErrorMessages.EMAIL_ALREADY_REGISTERED));
            }
        }

        if (result.IsValid == false)
        {
            var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();

            throw new ErrorsOnValidationException(errorMessages);
        }
    }
}
