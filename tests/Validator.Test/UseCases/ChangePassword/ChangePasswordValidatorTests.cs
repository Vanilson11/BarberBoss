using BarberBoss.Application.UseCases.Users.ChangePassword;
using BarberBoss.Exception;
using CommonTestsUtilities.Requests;
using Shouldly;

namespace Validator.Test.UseCases.ChangePassword;

public class ChangePasswordValidatorTests
{
    [Theory]
    [InlineData("")]
    [InlineData("    ")]
    [InlineData(null)]
    public void Error_New_Password_Empty(string password) 
    {
        var request = RequestChangePasswordJsonBuilder.Build();
        request.NewPassword = password;
        var validator = new ChangePasswordValidator();

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.Single().ShouldSatisfyAllConditions(error => error.ErrorMessage.ShouldBe(ResourceErrorMessages.PASSWORD_EMPTY));
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
    public void Error_New_Password_Invalid(string password) 
    {
        var request = RequestChangePasswordJsonBuilder.Build();
        request.NewPassword = password;
        var validator = new ChangePasswordValidator();

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.Single().ShouldSatisfyAllConditions(error => error.ErrorMessage.ShouldBe(ResourceErrorMessages.INVALID_PASSWORD));
    }
}
