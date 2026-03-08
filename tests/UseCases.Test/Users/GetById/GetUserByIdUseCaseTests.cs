using BarberBoss.Application.UseCases.Users.GetById;
using BarberBoss.Domain.Entities;
using BarberBoss.Exception;
using BarberBoss.Exception.ExceptionsBase;
using CommonTestsUtilities.AutoMapper;
using CommonTestsUtilities.Entities;
using CommonTestsUtilities.Repositories;
using Shouldly;

namespace UseCases.Test.Users.GetById;

public class GetUserByIdUseCaseTests
{
    private GetUserByIdUseCase CreateUseCase(User? user = null)
    {
        var readOnlyRepository = new ReadOnlyUsersRepositoryBuilder().GetById(user).Build();
        var mapper = AutoMappingBuilder.Build();

        return new GetUserByIdUseCase(readOnlyRepository, mapper);
    }

    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();
        var useCase = CreateUseCase(user);

        var result = await useCase.Execute(user.Id);

        result.ShouldNotBeNull();
        result.Name.ShouldBe(user.Name);
        result.Email.ShouldBe(user.Email);
        result.Role.ShouldBe(user.Role);
        result.CreatedAt.ShouldBe(user.CreatedAt);
        result.UpdatedAt.ShouldBe(user.UpdatedAt);
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
