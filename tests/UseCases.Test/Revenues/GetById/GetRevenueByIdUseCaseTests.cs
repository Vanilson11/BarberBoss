using BarberBoss.Application.UseCases.Revenues.GetById;
using BarberBoss.Domain.Entities;
using BarberBoss.Exception;
using BarberBoss.Exception.ExceptionsBase;
using CommonTestsUtilities.AutoMapper;
using CommonTestsUtilities.Entities;
using CommonTestsUtilities.Repositories;
using CommonTestsUtilities.Services.LoggedUser;
using Shouldly;

namespace UseCases.Test.Revenues.GetById;

public class GetRevenueByIdUseCaseTests
{
    private GetRevenueByIdUseCase CreateUseCase(User user, Revenue? revenue = null)
    {
        var readnOnlyRepository = new ReadOnlyRevenueRepositoryBuilder().GetById(user, revenue).Build();
        var mapper = AutoMappingBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);

        return new GetRevenueByIdUseCase(readnOnlyRepository, mapper, loggedUser);
    }

    [Fact]
    public async Task Success()
    {
        var loggedUser = UserBuilder.Build();
        var revenue = RevenueBuilder.Build(loggedUser);
        var useCase = CreateUseCase(loggedUser, revenue);

        var result = await useCase.Execute(revenue.Id);

        result.ShouldNotBeNull();
        result.Id.ShouldBe(revenue.Id);
        result.Title.ShouldBe(revenue.Title);
        result.Date.ShouldBe(revenue.Date);
        result.PaymentType.ShouldBe((BarberBoss.Communication.Enums.PaymentType)revenue.PaymentType);
        result.Amount.ShouldBe(revenue.Amount);
    }

    [Fact]
    public async Task Error_Revenue_Not_Found()
    {
        var loggedUser = UserBuilder.Build();
        var useCase = CreateUseCase(loggedUser);

        var act = async () => await useCase.Execute(id: 1000);

        var result = await act.ShouldThrowAsync<NotFoundException>();

        result.GetErrors().Single()
            .ShouldSatisfyAllConditions(error => error.ShouldBe(ResourceErrorMessages.REVENUE_NOT_FOUND));
    }
}
