using BarberBoss.Application.UseCases.Users.Register;
using BarberBoss.Communication.Requests;
using BarberBoss.Exception;
using BarberBoss.Exception.ExceptionsBase;
using CommonTestsUtilities.AutoMapper;
using CommonTestsUtilities.Repositories;
using CommonTestsUtilities.Requests;
using CommonTestsUtilities.Security.Criptography;
using Shouldly;

namespace UseCases.Test.Users.Register;
public class RegisterUserUseCaseTests
{
    private RegisterUserUseCase CreateUseCase(string? email = null)
    {
        var mapper = AutoMappingBuilder.Build();
        var passwordEnrypter = new PasswordEncrypterBuilder().Build();
        var readOnlyReposiotory = new ReadOnlyUsersRepositoryBuilder().EmailExists(email!).Build();
        var writeOnlyRepository = new WriteOnlyUsersRepositoryBuilder().Build();
        var unitOffWork = UnitOffWorkBuilder.Build();
        var tokenGenerator = TokenGeneratorBuilder.Build();

        return new RegisterUserUseCase(mapper, passwordEnrypter, readOnlyReposiotory, writeOnlyRepository, unitOffWork, tokenGenerator);
    }

    private RequestRegisterUserJson CreateRequest()
    {
        return RequestUserJsonBuilder.Build();
    }

    [Fact]
    public async void Success()
    {
        var request = CreateRequest();
        var useCase = CreateUseCase();

        var result = await useCase.Execute(request);

        result.ShouldNotBeNull();
        result.Name.ShouldBe(request.Name);
        result.Token.ShouldNotBeNullOrWhiteSpace();
    }

    [Fact]
    public async void Error_Name_Empty()
    {
        var request = CreateRequest();
        request.Name = string.Empty;
        var useCase = CreateUseCase();

        var act = async () => await useCase.Execute(request);

        var result = await act.ShouldThrowAsync<ErrorsOnValidationException>();
        result.GetErrors().Count.ShouldBe(1);

        var errorMessage = result.GetErrors().FirstOrDefault();
        errorMessage.ShouldBe(ResourceErrorMessages.NAME_EMPTY);
    }

    [Fact]
    public async void Error_Created_At_Be_The_Future()
    {
        var request = CreateRequest();
        request.CreatedAt = DateTime.UtcNow.AddDays(1);
        var useCase = CreateUseCase();

        var act = async () => await useCase.Execute(request);

        var result = await act.ShouldThrowAsync<ErrorsOnValidationException>();
        result.GetErrors().Count.ShouldBe(1);

        var errorMessage = result.GetErrors().FirstOrDefault();
        errorMessage.ShouldBe(ResourceErrorMessages.CREATED_AT_CANNOT_BE_THE_FUTURE);
    }

    [Fact]
    public async void Error_Email_Exists()
    {
        var request = CreateRequest();
        var useCase = CreateUseCase(request.Email);

        var act = async () => await useCase.Execute(request);

        var result = await act.ShouldThrowAsync<ErrorsOnValidationException>();
        result.GetErrors().Count.ShouldBe(1);

        var errorMessage = result.GetErrors().FirstOrDefault();
        errorMessage.ShouldBe(ResourceErrorMessages.EMAIL_ALREADY_REGISTERED);
    }

    [Fact]
    public async void Error_Password_Invalid()
    {
        var request = CreateRequest();
        request.Password = "aaaaaaa";
        var useCase = CreateUseCase();

        var act = async () => await useCase.Execute(request);

        var result = await act.ShouldThrowAsync<ErrorsOnValidationException>();
        result.GetErrors().Count.ShouldBe(1);

        var errorMessage = result.GetErrors().FirstOrDefault();
        errorMessage.ShouldBe(ResourceErrorMessages.INVALID_PASSWORD);
    }
}
