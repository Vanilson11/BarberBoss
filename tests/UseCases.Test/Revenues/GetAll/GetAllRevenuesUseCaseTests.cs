using BarberBoss.Application.UseCases.Revenues.GetAll;
using BarberBoss.Domain.Entities;
using CommonTestsUtilities.AutoMapper;
using CommonTestsUtilities.Entities;
using CommonTestsUtilities.Repositories;
using CommonTestsUtilities.Services.LoggedUser;
using Shouldly;

namespace UseCases.Test.Revenues.GetAll;

public class GetAllRevenuesUseCaseTests
{
    private GetAllRevenuesUseCase CreateUseCase(List<Revenue> revenues, User? user = null)
    {
        var readOnlyRepository = new ReadOnlyRevenueRepositoryBuilder().GetAllRevenues(user!, revenues).Build();
        var mapper = AutoMappingBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user!);

        return new GetAllRevenuesUseCase(readOnlyRepository, mapper, loggedUser);
    }

    [Fact]
    public async Task Success()
    {
        var loggedUser = UserBuilder.Build();
        var revenues = RevenueBuilder.Collection(loggedUser);
        var useCase = CreateUseCase(revenues, loggedUser);

        var result = await useCase.Execute();

        result.ShouldNotBeNull();
        result.Revenues.ShouldNotBeEmpty();
        result.Revenues.ForEach(revenue =>
        {
            revenue.Title.ShouldNotBeNullOrWhiteSpace();
            revenue.Date.ShouldBeLessThanOrEqualTo(DateTime.UtcNow.Date);
            revenue.Amount.ShouldBeGreaterThan(0);
        });
    }
}
