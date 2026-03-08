using BarberBoss.Communication.Requests;
using BarberBoss.Domain.Repositories;
using BarberBoss.Domain.Security.Criptography;
using BarberBoss.Domain.Services.LoggedUser;
using BarberBoss.Exception;
using BarberBoss.Exception.ExceptionsBase;
using FluentValidation.Results;

namespace BarberBoss.Application.UseCases.Users.ChangePassword;

public class ChangePasswordUseCase : IChangePasswordUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IUpdateOnlyUsersRepository _updateOnlyUsersRepository;
    private readonly IPasswordEncripter _passwordEncripter;
    private readonly IUnitOffWork _unitOffWork;

    public ChangePasswordUseCase(
        ILoggedUser loggedUser,
        IUpdateOnlyUsersRepository updateOnlyUsersRepository,
        IPasswordEncripter passwordEncripter,
        IUnitOffWork unitOffWork
        )
    {
        _loggedUser = loggedUser;
        _updateOnlyUsersRepository = updateOnlyUsersRepository;
        _passwordEncripter = passwordEncripter;
        _unitOffWork = unitOffWork;
    }
    public async Task Execute(RequestChangePasswordJson request)
    {
        var loggedUser = await _loggedUser.Get();

        Validate(request, loggedUser.Password);

        var user = await _updateOnlyUsersRepository.GetById(loggedUser.Id);

        user.Password = _passwordEncripter.Encrypt(request.NewPassword);

        user.UpdatedAt = DateTime.UtcNow;

        _updateOnlyUsersRepository.Update(user);

        await _unitOffWork.Commit();
    }

    private void Validate(RequestChangePasswordJson request, string currentPassword)
    {
        var result = new ChangePasswordValidator().Validate(request);

        var passwordMatch = _passwordEncripter.Verify(request.Password, currentPassword);

        if(passwordMatch is false)
        {
            result.Errors.Add(new ValidationFailure(string.Empty, ResourceErrorMessages.PASSWORD_DIFFERENT_CURRENT_PASSWORD));
        }

        if(result.IsValid is false)
        {
            var errorMessages = result.Errors.Select(x => x.ErrorMessage).ToList();

            throw new ErrorsOnValidationException(errorMessages);
        }
    }
}
