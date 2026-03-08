using BarberBoss.Application.UseCases.Users;
using BarberBoss.Communication.Requests;
using CommonTestsUtilities.Requests;
using FluentValidation;
using Shouldly;

namespace Validator.Test.UseCases.Users;
public class PasswordValidatorTests
{
    private PasswordValidator<RequestRegisterUserJson> CreateValidator()
    {
        return new PasswordValidator<RequestRegisterUserJson>();
    }
    private RequestRegisterUserJson CreateRequest() => RequestUserJsonBuilder.Build();

    [Theory]
    [InlineData("")]
    [InlineData("    ")]
    [InlineData(null)]
    public void Error_Password_Empty(string password)
    {
        var validator = CreateValidator();
        var request = CreateRequest();
        request.Password = password;

        var result = validator.IsValid(new ValidationContext<RequestRegisterUserJson>(new RequestRegisterUserJson()), password);

        result.ShouldBeFalse();
    }

    [Theory]
    [InlineData("a")]
    [InlineData("aa")]
    [InlineData("aaa")]
    [InlineData("aaaa")]
    [InlineData("aaaaa")]
    [InlineData("aaaaaa")]
    [InlineData("aaaaaaa")]
    [InlineData("aaaaaaaa")]
    [InlineData("AAAAAAAA")]
    [InlineData("Aaaaaaaa")]
    [InlineData("Aaaaaaa1")]
    public void Error_Password_Invalid(string password)
    {
        var validator = CreateValidator();
        var request = CreateRequest();
        request.Password = password;

        var result = validator.IsValid(new ValidationContext<RequestRegisterUserJson>(new RequestRegisterUserJson()), password);

        result.ShouldBeFalse();
    }
}
