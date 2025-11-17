using BarberBoss.Application.UseCases.DoLogin;
using BarberBoss.Communication.Requests;
using BarberBoss.Domain.Entities;
using BarberBoss.Exception;
using BarberBoss.Exception.ExceptionsBase;
using CommonTestsUtilities.Entities;
using CommonTestsUtilities.Repositories;
using CommonTestsUtilities.Requests;
using CommonTestsUtilities.Security.Criptography;
using Shouldly;

namespace UseCases.Test.Login.DoLogin;
public class DoLoginUseCaseTests
{
    private DoLoginUseCase CreateUseCase(User? user, string? password = null)
    {
        var readOnlyRepository = new ReadOnlyUsersRepositoryBuilder().GetUserByEmail(user!).Build();
        var passwordEncrypter = new PasswordEncrypterBuilder().Verify(password!).Build();
        var tokenGenerator = TokenGeneratorBuilder.Build();
        return new DoLoginUseCase(readOnlyRepository, passwordEncrypter, tokenGenerator);
    }

    private RequestDoLoginJson CreateRequest()
    {
        return RequestLoginUseCaseBuilder.Build();
    }

    [Fact]
    private async Task Success()
    {
        var user = UserBuilder.Build();
        var request = CreateRequest();
        request.Email = user.Email;
        var useCase = CreateUseCase(user, request.Password);

        var result = await useCase.Execute(request);

        result.ShouldNotBeNull();
        result.Name.ShouldBe(user.Name);
        result.Token.ShouldNotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Error_User_Not_Exists()
    {
        var request = CreateRequest();
        var useCase = CreateUseCase(null, request.Password);

        var act = async () => await useCase.Execute(request);

        var result = await act.ShouldThrowAsync<InvalidLoginException>();
        result.GetErrors().Count.ShouldBe(1);
        var errorMessage = result.GetErrors().FirstOrDefault();
        errorMessage.ShouldBe(ResourceErrorMessages.EMAIL_OR_PASSWORD_INVALID);
    }

    [Fact]
    public async Task Error_Passwordo_Not_Match()
    {
        var user = UserBuilder.Build();
        var request = CreateRequest();
        request.Email = user.Email;
        var useCase = CreateUseCase(user);

        var act = async () => await useCase.Execute(request);

        var result = await act.ShouldThrowAsync<InvalidLoginException>();
        result.GetErrors().Count.ShouldBe(1);
        var errorMessage = result.GetErrors().FirstOrDefault();
        errorMessage.ShouldBe(ResourceErrorMessages.EMAIL_OR_PASSWORD_INVALID);
    }
}
