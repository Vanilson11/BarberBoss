using BarberBoss.Communication.Requests;
using BarberBoss.Domain.Repositories;
using BarberBoss.Domain.Services.LoggedUser;
using BarberBoss.Exception;
using BarberBoss.Exception.ExceptionsBase;
using FluentValidation.Results;

namespace BarberBoss.Application.UseCases.Users.UpdateProfile;

public class UpdateProfileUserUseCase : IUpdateProfileUserUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IUpdateOnlyUsersRepository _updateOnlyUsersRepository;
    private readonly IUnitOffWork _unitOffWork;
    private readonly IReadOnlyUsersRepository _readOnlyUsersRepository;

    public UpdateProfileUserUseCase(
        ILoggedUser loggedUser,
        IUpdateOnlyUsersRepository updateOnlyUsersRepository,
        IUnitOffWork unitOffWork,
        IReadOnlyUsersRepository readOnlyUsersRepository
        )
    {
        _loggedUser = loggedUser;
        _unitOffWork = unitOffWork;
        _updateOnlyUsersRepository = updateOnlyUsersRepository;
        _readOnlyUsersRepository = readOnlyUsersRepository;
    }
    public async Task Execute(RequestUpdateUserProfileJson request)
    {
        var loggedUser = await _loggedUser.Get();

        await Validate(request, loggedUser.Email);

        var user = await _updateOnlyUsersRepository.GetById(loggedUser.Id);

        user.Name = request.Name;
        user.Email = request.Email;
        user.UpdatedAt = request.UpdatedAt;

        _updateOnlyUsersRepository.Update(user);

        await _unitOffWork.Commit();
    }

    private async Task Validate(RequestUpdateUserProfileJson request, string currentEmail)
    {
        var result = new UpdateUserProfileValidator().Validate(request);

        if(currentEmail.Equals(request.Email) == false)
        {
            var emailAlreadyExists = await _readOnlyUsersRepository.EmailExists(request.Email);

            if (emailAlreadyExists)
            {
                result.Errors.Add(new ValidationFailure(string.Empty, ResourceErrorMessages.EMAIL_ALREADY_REGISTERED));
            }
        }

        if (result.IsValid is false)
        {
            var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();

            throw new ErrorsOnValidationException(errorMessages);
        }
    }
}
