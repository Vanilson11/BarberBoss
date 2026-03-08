using BarberBoss.Application.UseCases.Users.Delete;
using BarberBoss.Domain.Entities;
using BarberBoss.Exception;
using BarberBoss.Exception.ExceptionsBase;
using CommonTestsUtilities.Entities;
using CommonTestsUtilities.Repositories;
using Shouldly;

namespace UseCases.Test.Users.Delete;

public class DeleteUserUseCaseTests
{
    private DeleteUserUseCase CreateUseCase(User? user = null)
    {
        var writeOnlyRepository = new WriteOnlyUsersRepositoryBuilder().Delete(user).Build();
        var unitOffWork = UnitOffWorkBuilder.Build();

        return new DeleteUserUseCase(writeOnlyRepository, unitOffWork);
    }

    [Fact]
    public async Task Success()
    {
        var loggedUser = UserBuilder.Build();
        var useCase = CreateUseCase(loggedUser);

        var act = async () => await useCase.Execute(loggedUser.Id);

        await act.ShouldNotThrowAsync();
    }

    [Fact]
    public async Task Error_User_Not_Found()
    {
        var useCase = CreateUseCase();

        var act = async () => await useCase.Execute(id: 1000);

        var result = await act.ShouldThrowAsync<NotFoundException>();

        result.GetErrors().Single()
            .ShouldSatisfyAllConditions(error => error.ShouldBe(ResourceErrorMessages.USER_NOT_FOUND));
    }
}
