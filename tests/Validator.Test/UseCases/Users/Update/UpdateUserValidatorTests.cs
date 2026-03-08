using BarberBoss.Application.UseCases.Users.Update;
using BarberBoss.Exception;
using CommonTestsUtilities.Requests;
using Shouldly;

namespace Validator.Test.UseCases.Users.Update;

public class UpdateUserValidatorTests
{
    [Fact]
    public void Success()
    {
        var request = RequestUpdateUserJsonBuilder.Build();
        
        var result = new UpdateUserValidator().Validate(request);

        result.IsValid.ShouldBeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Error_Name_Empty(string name)
    {
        var request = RequestUpdateUserJsonBuilder.Build();
        request.Name = name;

        var result = new UpdateUserValidator().Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.Single()
            .ShouldSatisfyAllConditions(error => error.ErrorMessage.ShouldBe(ResourceErrorMessages.NAME_EMPTY));
    }

    [Fact]
    public void Error_Name_Less_Than_Two_Characters()
    {
        var request = RequestUpdateUserJsonBuilder.Build();
        request.Name = "a";

        var result = new UpdateUserValidator().Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.Single()
            .ShouldSatisfyAllConditions(error => error.ErrorMessage.ShouldBe(ResourceErrorMessages.NAME_MIN_LENGTH));
    }

    [Fact]
    public void Error_Name_More_Than_One_Hundred_Characters()
    {
        var request = RequestUpdateUserJsonBuilder.Build();
        request.Name = new string('a', 101);

        var result = new UpdateUserValidator().Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.Single()
            .ShouldSatisfyAllConditions(error => error.ErrorMessage.ShouldBe(ResourceErrorMessages.NAME_MAX_LENGTH));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Error_Email_Empty(string email)
    {
        var request = RequestUpdateUserJsonBuilder.Build();
        request.Email = email;

        var result = new UpdateUserValidator().Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.Single()
            .ShouldSatisfyAllConditions(error => error.ErrorMessage.ShouldBe(ResourceErrorMessages.EMAIL_EMPTY));
    }

    [Fact]
    public void Error_Email_Invalid()
    {
        var request = RequestUpdateUserJsonBuilder.Build();
        request.Email = "invalid_email.com";

        var result = new UpdateUserValidator().Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.Single()
            .ShouldSatisfyAllConditions(error => error.ErrorMessage.ShouldBe(ResourceErrorMessages.EMAIL_INVALID));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Error_Role_Empty(string role)
    {
        var request = RequestUpdateUserJsonBuilder.Build();
        request.Role = role;

        var result = new UpdateUserValidator().Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.Single()
            .ShouldSatisfyAllConditions(error => error.ErrorMessage.ShouldBe(ResourceErrorMessages.ROLE_EMPTY));
    }

    [Theory]
    [InlineData("abcd")]
    [InlineData("efgh")]
    public void Error_Role_Invalid(string role)
    {
        var request = RequestUpdateUserJsonBuilder.Build();
        request.Role = role;

        var result = new UpdateUserValidator().Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.Single()
            .ShouldSatisfyAllConditions(error => error.ErrorMessage.ShouldBe(ResourceErrorMessages.ROLE_INVALID));
    }

    [Fact]
    public void Error_Created_At_The_Future()
    {
        var request = RequestUpdateUserJsonBuilder.Build();
        request.CreatedAt = DateTime.UtcNow.AddDays(1);

        var result = new UpdateUserValidator().Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.Single()
            .ShouldSatisfyAllConditions(error => error.ErrorMessage.ShouldBe(ResourceErrorMessages.CREATED_AT_CANNOT_BE_THE_FUTURE));
    }
}
