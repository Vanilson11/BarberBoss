using BarberBoss.Application.UseCases.Users.UpdateProfile;
using BarberBoss.Domain.Entities;
using BarberBoss.Exception;
using BarberBoss.Exception.ExceptionsBase;
using CommonTestsUtilities.Entities;
using CommonTestsUtilities.Repositories;
using CommonTestsUtilities.Requests;
using CommonTestsUtilities.Services.LoggedUser;
using Shouldly;

namespace UseCases.Test.Users.UpdateProfile;

public class UpdateProfileUserUseCaseTests
{
    private UpdateProfileUserUseCase CreateUseCase(User user, string? email = null)
    {
        var loggedUser = LoggedUserBuilder.Build(user);
        var updateOnlyRepository = new UpdateOnlyUsersRepositoryBuilder().GetById(user).Build();
        var unitOffWork = UnitOffWorkBuilder.Build();
        var readOnlyRepository = new ReadOnlyUsersRepositoryBuilder().EmailExists(email).Build();

        return new UpdateProfileUserUseCase(loggedUser, updateOnlyRepository, unitOffWork, readOnlyRepository);
    }

    [Fact]
    public async Task Success()
    {
        var loggedUser = UserBuilder.Build();
        var request = RequestUpdateUserProfileJsonBuilder.Build();
        var useCase = CreateUseCase(loggedUser);

        var act = async () => await useCase.Execute(request);

        await act.ShouldNotThrowAsync();

        loggedUser.Name.ShouldBe(request.Name);
        loggedUser.Email.ShouldBe(request.Email);
        loggedUser.UpdatedAt.ShouldBe(request.UpdatedAt);
    }

    [Fact]
    public async Task Error_Name_Empty()
    {
        var loggedUser = UserBuilder.Build();
        var request = RequestUpdateUserProfileJsonBuilder.Build();
        request.Name = string.Empty;
        var useCase = CreateUseCase(loggedUser);

        var act = async () => await useCase.Execute(request);

        var result = await act.ShouldThrowAsync<ErrorsOnValidationException>();

        result.GetErrors().Single()
            .ShouldSatisfyAllConditions(error => error.ShouldBe(ResourceErrorMessages.NAME_EMPTY));
    }

    [Fact]
    public async Task Error_Email_Exists()
    {
        var loggedUser = UserBuilder.Build();
        var request = RequestUpdateUserProfileJsonBuilder.Build();
        var useCase = CreateUseCase(loggedUser, request.Email);

        var act = async () => await useCase.Execute(request);

        var result = await act.ShouldThrowAsync<ErrorsOnValidationException>();

        result.GetErrors().Single()
            .ShouldSatisfyAllConditions(error => error.ShouldBe(ResourceErrorMessages.EMAIL_ALREADY_REGISTERED));
    }
}
