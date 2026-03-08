using BarberBoss.Application.UseCases.Revenues.Register;
using BarberBoss.Domain.Entities;
using BarberBoss.Exception;
using BarberBoss.Exception.ExceptionsBase;
using CommonTestsUtilities.AutoMapper;
using CommonTestsUtilities.Entities;
using CommonTestsUtilities.Repositories;
using CommonTestsUtilities.Requests;
using CommonTestsUtilities.Services.LoggedUser;
using Shouldly;

namespace UseCases.Test.Revenues.Register;

public class RegisterRevenueUseCaseTests
{
    private RegisterRevenueUseCase CreateUseCase(User? user = null)
    {
        var writeOnlyRepository = WriteOnlyRevenueRepositoryBuilder.Build();
        var unitOffWork = UnitOffWorkBuilder.Build();
        var mapper = AutoMappingBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user!);

        return new RegisterRevenueUseCase(writeOnlyRepository, unitOffWork, mapper, loggedUser);
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestRevenuesJsonBuilder.Build();
        var loggedUser = UserBuilder.Build();
        var useCase = CreateUseCase(loggedUser);

        var response = await useCase.Execute(request);

        response.ShouldNotBeNull();
        response.Title.ShouldBe(request.Title);
    }

    [Fact]
    public async Task Error_Title_Empty()
    {
        var request = RequestRevenuesJsonBuilder.Build();
        request.Title = string.Empty;
        var loggedUser = UserBuilder.Build();
        var useCase = CreateUseCase(loggedUser);

        var act = async () => await useCase.Execute(request);

        var result = await act.ShouldThrowAsync<ErrorsOnValidationException>();

        result.GetErrors().Single().ShouldSatisfyAllConditions(error => error.ShouldBe(ResourceErrorMessages.TITLE_REQUIRED));
    }
}
