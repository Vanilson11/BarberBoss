using BarberBoss.Application.UseCases.Users.ChangePassword;
using BarberBoss.Exception;
using CommonTestsUtilities.Requests;
using Shouldly;

namespace Validator.Test.UseCases.Users.ChangePassword;

public class ChangePasswordValidatorTests
{
    [Fact]
    public void Success()
    {
        var request = RequestChangePasswordJsonBuilder.Build();

        var result = new ChangePasswordValidator().Validate(request);

        result.IsValid.ShouldBeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Error_Password_Empty(string password)
    {
        var request = RequestChangePasswordJsonBuilder.Build();
        request.NewPassword = password;

        var result = new ChangePasswordValidator().Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.Single()
            .ShouldSatisfyAllConditions(error => error.ErrorMessage.ShouldBe(ResourceErrorMessages.PASSWORD_EMPTY));
    }
}
