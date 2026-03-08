using BarberBoss.Application.UseCases.Revenues.Update;
using BarberBoss.Domain.Entities;
using BarberBoss.Exception;
using BarberBoss.Exception.ExceptionsBase;
using CommonTestsUtilities.AutoMapper;
using CommonTestsUtilities.Entities;
using CommonTestsUtilities.Repositories;
using CommonTestsUtilities.Requests;
using CommonTestsUtilities.Services.LoggedUser;
using Shouldly;

namespace UseCases.Test.Revenues.Update;

public class UpdateRevenueUseCaseTests
{
    private UpdateRevenueUseCase CreateUseCase(User user, Revenue? revenue = null)
    {
        var updateOnlyRepository = new UpdateRevenueUseCaseBuilder().GetById(user, revenue).Build();
        var unitOffWork = UnitOffWorkBuilder.Build();
        var mapper = AutoMappingBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);

        return new UpdateRevenueUseCase(updateOnlyRepository, unitOffWork, mapper, loggedUser);
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestRevenuesJsonBuilder.Build();
        var loggedUser = UserBuilder.Build();
        var revenue = RevenueBuilder.Build(loggedUser);
        var useCase = CreateUseCase(loggedUser, revenue);

        var act = async () => await useCase.Execute(request, revenue.Id);

        await act.ShouldNotThrowAsync();
    }
    
    [Fact]
    public async Task Error_Title_Empty()
    {
        var request = RequestRevenuesJsonBuilder.Build();
        request.Title = string.Empty;
        var loggedUser = UserBuilder.Build();
        var revenue = RevenueBuilder.Build(loggedUser);
        var useCase = CreateUseCase(loggedUser, revenue);

        var act = async () => await useCase.Execute(request, revenue.Id);

        var result = await act.ShouldThrowAsync<ErrorsOnValidationException>();

        result.GetErrors().Single()
            .ShouldSatisfyAllConditions(error => error.ShouldBe(ResourceErrorMessages.TITLE_REQUIRED));
    }

    [Fact]
    public async Task Error_Revenue_Not_Found()
    {
        var request = RequestRevenuesJsonBuilder.Build();
        var loggedUser = UserBuilder.Build();
        var useCase = CreateUseCase(loggedUser);

        var act = async () => await useCase.Execute(request, id: 1000);

        var result = await act.ShouldThrowAsync<NotFoundException>();

        result.GetErrors().Single()
            .ShouldSatisfyAllConditions(error => error.ShouldBe(ResourceErrorMessages.REVENUE_NOT_FOUND));
    }
}
