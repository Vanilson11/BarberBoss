using BarberBoss.Application.UseCases.Users;
using BarberBoss.Communication.Requests;
using BarberBoss.Exception;
using CommonTestsUtilities.Requests;
using Shouldly;

namespace Validator.Test.UseCases.Users.Register;
public class RegisterUserUseCaseTests
{
    private RequestUserJson CreateRequest()
    {
        return RequestUserJsonBuilder.Build();
    }
    private UserUseCaseValidator CreateValidator() => new UserUseCaseValidator();

    [Fact]
    public void Success()
    {
        var request = CreateRequest();
        var validator = CreateValidator();

        var result = validator.Validate(request);

        result.IsValid.ShouldBeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("    ")]
    [InlineData(null)]
    public void Error_Name_Empty(string name)
    {
        var request = CreateRequest();
        var validator = CreateValidator();
        request.Name = name;

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.Single().ShouldSatisfyAllConditions(erro => erro.ErrorMessage.Equals(ResourceErrorMessages.NAME_EMPTY));
    }

    [Theory]
    [InlineData("")]
    [InlineData("    ")]
    [InlineData(null)]
    public void Error_Email_Empty(string email)
    {
        var request = CreateRequest();
        var validator = CreateValidator();
        request.Email = email;

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.Single().ShouldSatisfyAllConditions(erro => erro.ErrorMessage.Equals(ResourceErrorMessages.EMAIL_EMPTY));
    }

    [Fact]
    public void Error_Email_Invalid()
    {
        var validator = CreateValidator();
        var request = CreateRequest();
        request.Email = "vanilson.com";

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.Single().ShouldSatisfyAllConditions(erro => erro.ErrorMessage.Equals(ResourceErrorMessages.EMAIL_INVALID));
    }
}
