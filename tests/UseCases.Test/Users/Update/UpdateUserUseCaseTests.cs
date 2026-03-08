using BarberBoss.Application.UseCases.Users.Update;
using BarberBoss.Domain.Entities;
using BarberBoss.Exception;
using BarberBoss.Exception.ExceptionsBase;
using CommonTestsUtilities.AutoMapper;
using CommonTestsUtilities.Entities;
using CommonTestsUtilities.Repositories;
using CommonTestsUtilities.Requests;
using Shouldly;

namespace UseCases.Test.Users.Update;

public class UpdateUserUseCaseTests
{
    private UpdateUserUseCase CreateUseCase(User? user = null, string? email = null)
    {
        var updateOnlyRepository = new UpdateOnlyUsersRepositoryBuilder().Build();
        var readOnlyRepository = new ReadOnlyUsersRepositoryBuilder().EmailExists(email).GetById(user).Build();
        var mapper = AutoMappingBuilder.Build();
        var unitOffWork = UnitOffWorkBuilder.Build();

        return new UpdateUserUseCase(updateOnlyRepository, readOnlyRepository, mapper, unitOffWork);
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestUpdateUserJsonBuilder.Build();
        var user = UserBuilder.Build();
        var useCase = CreateUseCase(user);

        var act = async () => await useCase.Execute(user.Id, request);

        await act.ShouldNotThrowAsync();
    }

    [Fact]
    public async Task Error_User_Not_Found()
    {
        var request = RequestUpdateUserJsonBuilder.Build();
        var useCase = CreateUseCase();

        var act = async () => await useCase.Execute(id: 1000, request);

        var result = await act.ShouldThrowAsync<NotFoundException>();

        result.GetErrors().Single()
            .ShouldSatisfyAllConditions(error => error.ShouldBe(ResourceErrorMessages.USER_NOT_FOUND));
    }

    [Fact]
    public async Task Error_Name_Empty()
    {
        var request = RequestUpdateUserJsonBuilder.Build();
        request.Name = string.Empty;
        var user = UserBuilder.Build();
        var useCase = CreateUseCase(user);

        var act = async () => await useCase.Execute(user.Id, request);

        var result = await act.ShouldThrowAsync<ErrorsOnValidationException>();

        result.GetErrors().Single()
            .ShouldSatisfyAllConditions(error => error.ShouldBe(ResourceErrorMessages.NAME_EMPTY));
    }

    [Fact]
    public async Task Error_Email_Exists()
    {
        var request = RequestUpdateUserJsonBuilder.Build();
        var user = UserBuilder.Build();
        var useCase = CreateUseCase(user, request.Email);

        var act = async () => await useCase.Execute(user.Id, request);

        var result = await act.ShouldThrowAsync<ErrorsOnValidationException>();

        result.GetErrors().Single()
            .ShouldSatisfyAllConditions(error => error.ShouldBe(ResourceErrorMessages.EMAIL_ALREADY_REGISTERED));
    }
}
