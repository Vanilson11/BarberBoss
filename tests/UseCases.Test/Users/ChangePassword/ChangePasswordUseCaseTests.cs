using BarberBoss.Application.UseCases.Users.ChangePassword;
using BarberBoss.Domain.Entities;
using BarberBoss.Exception;
using BarberBoss.Exception.ExceptionsBase;
using CommonTestsUtilities.Entities;
using CommonTestsUtilities.Repositories;
using CommonTestsUtilities.Requests;
using CommonTestsUtilities.Security.Criptography;
using CommonTestsUtilities.Services.LoggedUser;
using Shouldly;

namespace UseCases.Test.Users.ChangePassword;

public class ChangePasswordUseCaseTests
{
    private ChangePasswordUseCase CreateUseCase(User user, string? requestPassword = null)
    {
        var loggedUser = LoggedUserBuilder.Build(user);
        var updateOnlyRepository = new UpdateOnlyUsersRepositoryBuilder().GetById(user).Build();
        var passwordEncrypter = new PasswordEncrypterBuilder().Verify(requestPassword).Build();
        var unitOffWork = UnitOffWorkBuilder.Build();

        return new ChangePasswordUseCase(loggedUser, updateOnlyRepository, passwordEncrypter, unitOffWork);
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestChangePasswordJsonBuilder.Build();
        var loggedUser = UserBuilder.Build();
        var useCase = CreateUseCase(loggedUser, request.Password);

        var act = async () => await useCase.Execute(request);

        await act.ShouldNotThrowAsync();
    }

    [Fact]
    public async Task Error_New_Password_Empty()
    {
        var request = RequestChangePasswordJsonBuilder.Build();
        request.NewPassword = string.Empty;
        var loggedUser = UserBuilder.Build();
        var useCase = CreateUseCase(loggedUser, request.Password);

        var act = async () => await useCase.Execute(request);

        var result = await act.ShouldThrowAsync<ErrorsOnValidationException>();

        result.GetErrors().Single()
            .ShouldSatisfyAllConditions(error => error.ShouldBe(ResourceErrorMessages.PASSWORD_EMPTY));
    }

    [Fact]
    public async Task Error_Current_Password_Different()
    {
        var request = RequestChangePasswordJsonBuilder.Build();
        var loggedUser = UserBuilder.Build();
        var useCase = CreateUseCase(loggedUser);

        var act = async () => await useCase.Execute(request);

        var result = await act.ShouldThrowAsync<ErrorsOnValidationException>();

        result.GetErrors().Single()
            .ShouldSatisfyAllConditions(error => error.ShouldBe(ResourceErrorMessages.PASSWORD_DIFFERENT_CURRENT_PASSWORD));
    }
}
