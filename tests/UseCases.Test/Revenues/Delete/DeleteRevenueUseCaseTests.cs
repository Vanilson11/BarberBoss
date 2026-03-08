using BarberBoss.Application.UseCases.Revenues.Delete;
using BarberBoss.Domain.Entities;
using BarberBoss.Exception;
using BarberBoss.Exception.ExceptionsBase;
using CommonTestsUtilities.Entities;
using CommonTestsUtilities.Repositories;
using CommonTestsUtilities.Services.LoggedUser;
using Shouldly;

namespace UseCases.Test.Revenues.Delete;

public class DeleteRevenueUseCaseTests
{
    private DeleteRevenueUseCase CreateUseCase(User user, Revenue? revenue = null)
    {
        var writeOnlyRepositories = WriteOnlyRevenueRepositoryBuilder.Build();
        var unitOffWork = UnitOffWorkBuilder.Build();
        var readOnlyRepository = new ReadOnlyRevenueRepositoryBuilder().GetById(user, revenue).Build();
        var loggedUser = LoggedUserBuilder.Build(user);

        return new DeleteRevenueUseCase(writeOnlyRepositories, unitOffWork, readOnlyRepository, loggedUser);
    }

    [Fact]
    public async Task Success()
    {
        var loggedUser = UserBuilder.Build();
        var revenue = RevenueBuilder.Build(loggedUser);
        var useCase = CreateUseCase(loggedUser, revenue);

        var act = async () => await useCase.Execute(revenue.Id);

        await act.ShouldNotThrowAsync();
    }

    [Fact]
    public async Task Error_Revenue_Not_Found()
    {
        var loggedUser = UserBuilder.Build();
        var useCae = CreateUseCase(loggedUser);

        var act = async () => await useCae.Execute(1000);

        var result = await act.ShouldThrowAsync<NotFoundException>();

        result.GetErrors().Single()
            .ShouldSatisfyAllConditions(error => error.ShouldBe(ResourceErrorMessages.REVENUE_NOT_FOUND));
    }
}
