using BarberBoss.Application.UseCases.Users.GetProfile;
using BarberBoss.Domain.Entities;
using CommonTestsUtilities.AutoMapper;
using CommonTestsUtilities.Entities;
using CommonTestsUtilities.Services.LoggedUser;
using Shouldly;

namespace UseCases.Test.Users.GetProfile;

public class GetProfileUserUseCaseTests
{
    private GetProfileUserUseCase CreateUseCase(User user)
    {
        var loggedUser = LoggedUserBuilder.Build(user);
        var mapper = AutoMappingBuilder.Build();

        return new GetProfileUserUseCase(loggedUser, mapper);
    }

    [Fact]
    public async Task Success()
    {
        var loggedUser = UserBuilder.Build();
        var useCase = CreateUseCase(loggedUser);

        var result = await useCase.Execute();

        result.ShouldNotBeNull();
        result.Name.ShouldBe(loggedUser.Name);
        result.Email.ShouldBe(loggedUser.Email);
        result.Role.ShouldBe(loggedUser.Role);
        result.CreatedAt.ShouldBe(loggedUser.CreatedAt);
        result.UpdatedAt.ShouldBe(loggedUser.UpdatedAt);
    }
}
