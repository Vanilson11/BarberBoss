using BarberBoss.Application.UseCases.Users.UpdateProfile;
using BarberBoss.Exception;
using CommonTestsUtilities.Requests;
using DocumentFormat.OpenXml.Spreadsheet;
using Shouldly;

namespace Validator.Test.UseCases.Users.UpdateProfile;

public class UpdateUserProfileValidatorTests
{
    [Fact]
    public void Success()
    {
        var request = RequestUpdateUserProfileJsonBuilder.Build();
        var validator = new UpdateUserProfileValidator();

        var result = validator.Validate(request);

        result.IsValid.ShouldBeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("    ")]
    [InlineData(null)]
    public void Error_Name_Empty(string name)
    {
        var request = RequestUpdateUserProfileJsonBuilder.Build();
        request.Name = name;
        var validator = new UpdateUserProfileValidator();

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.Single().ShouldSatisfyAllConditions(error => error.ErrorMessage.ShouldBe(ResourceErrorMessages.NAME_EMPTY));
    }

    [Fact]
    public void Error_Name_Less_Tha_Two_Characters()
    {
        var request = RequestUpdateUserProfileJsonBuilder.Build();
        request.Name = "a";
        var validator = new UpdateUserProfileValidator();

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.Single().ShouldSatisfyAllConditions(error => error.ErrorMessage.ShouldBe(ResourceErrorMessages.NAME_MIN_LENGTH));
    }

    [Fact]
    public void Error_Name_More_Tha_One_Hundred_Characters()
    {
        var request = RequestUpdateUserProfileJsonBuilder.Build();
        request.Name = new string('A', 101);
        var validator = new UpdateUserProfileValidator();

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.Single().ShouldSatisfyAllConditions(error => error.ErrorMessage.ShouldBe(ResourceErrorMessages.NAME_MAX_LENGTH));
    }

    [Theory]
    [InlineData("")]
    [InlineData("    ")]
    [InlineData(null)]
    public void Error_Email_Empty(string email)
    {
        var request = RequestUpdateUserProfileJsonBuilder.Build();
        request.Email = email;
        var validator = new UpdateUserProfileValidator();

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.Single().ShouldSatisfyAllConditions(error => error.ErrorMessage.ShouldBe(ResourceErrorMessages.EMAIL_EMPTY));
    }

    [Fact]
    public void Error_Email_Invalid()
    {
        var request = RequestUpdateUserProfileJsonBuilder.Build();
        request.Email = "email.invalid.com";
        var validator = new UpdateUserProfileValidator();

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.Single().ShouldSatisfyAllConditions(error => error.ErrorMessage.ShouldBe(ResourceErrorMessages.EMAIL_INVALID));
    }
}
