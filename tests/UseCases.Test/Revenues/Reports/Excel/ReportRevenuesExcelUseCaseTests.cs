using BarberBoss.Application.UseCases.Revenues.Reports.Excel;
using BarberBoss.Domain.Entities;
using CommonTestsUtilities.Entities;
using CommonTestsUtilities.Repositories;
using CommonTestsUtilities.Services.LoggedUser;
using Shouldly;

namespace UseCases.Test.Revenues.Reports.Excel;

public class ReportRevenuesExcelUseCaseTests
{
    private ReportRevenuesExcelUseCase CreateUseCase(User user, List<Revenue> revenues)
    {
        var readOnlyRepository = new ReportsReadOnlyRepositoryBuilder().GetByWeek(user, revenues).Build();
        var loggedUser = LoggedUserBuilder.Build(user);

        return new ReportRevenuesExcelUseCase(readOnlyRepository, loggedUser);
    }

    [Fact]
    public async Task Success()
    {
        var loggedUser = UserBuilder.Build();
        var revenues = RevenueBuilder.Collection(loggedUser);
        var useCase = CreateUseCase(loggedUser, revenues);
        var dateStart = DateOnly.FromDateTime(DateTime.UtcNow.Date);
        var dateEnd = DateOnly.FromDateTime(DateTime.UtcNow.Date).AddDays(2);

        var result = await useCase.Execute(dateStart, dateEnd);

        result.ShouldNotBeNull();
        result.ShouldNotBeEmpty();
    }

    [Fact]
    public async Task SuccessEmpty()
    {
        var loggedUser = UserBuilder.Build();
        var useCase = CreateUseCase(loggedUser, []);
        var dateStart = DateOnly.FromDateTime(DateTime.UtcNow.Date);
        var dateEnd = DateOnly.FromDateTime(DateTime.UtcNow.Date).AddDays(2);

        var result = await useCase.Execute(dateStart, dateEnd);

        result.ShouldBeEmpty();
    }
}
