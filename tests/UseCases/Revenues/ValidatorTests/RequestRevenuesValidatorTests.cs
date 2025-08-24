using BarberBoss.Application.UseCases.Revenues;
using BarberBoss.Exception;
using CommonTestsUtilities;
using Shouldly;

namespace ValidatorTests;
public class RequestRevenuesValidatorTests
{
    [Fact]
    public void Success()
    {
        var request = new RequestRevenuesJsonBuilder().Build();
        var validator = new RevenueUseCaseValidator();

        var result = validator.Validate(request);

        result.IsValid.ShouldBe(true);
    }

    [Fact]
    public void Error_Title_Empty()
    {
        var request = new RequestRevenuesJsonBuilder().Build();
        var validator = new RevenueUseCaseValidator();
        request.Title = string.Empty;

        var result = validator.Validate(request);

        result.IsValid.ShouldBe(false);
        result.Errors.Single()
            .ShouldSatisfyAllConditions(errorMessage => errorMessage.ErrorMessage.ShouldBe(ResourceErrorMessages.TITLE_REQUIRED));
    }

    [Fact]
    public void Error_Date_For_The_Future()
    {
        var request = new RequestRevenuesJsonBuilder().Build();
        var validator = new RevenueUseCaseValidator();
        request.Date = DateTime.UtcNow.AddDays(1);

        var result = validator.Validate(request);

        result.IsValid.ShouldBe(false);
        result.Errors.Single()
            .ShouldSatisfyAllConditions(error => error.ErrorMessage.ShouldBe(ResourceErrorMessages.REVENUE_CANNOT_BE_THE_FUTURE));
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(5)]
    [InlineData(9)]
    [InlineData(6)]
    public void Error_PaymentType_Invalid(int paymentType)
    {
        var request = new RequestRevenuesJsonBuilder().Build();
        var validator = new RevenueUseCaseValidator();
        request.PaymentType = (BarberBoss.Communication.Enums.PaymentType)paymentType;

        var result = validator.Validate(request);

        result.IsValid.ShouldBe(false);
        result.Errors.Single()
            .ShouldSatisfyAllConditions(error => error.ErrorMessage.ShouldBe(ResourceErrorMessages.PAYMENT_TYPE_INVALID));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-10)]
    public void Error_Amount_Invalid(decimal amount)
    {
        var request = new RequestRevenuesJsonBuilder().Build();
        var validator = new RevenueUseCaseValidator();
        request.Amount = amount;

        var result = validator.Validate(request);

        result.IsValid.ShouldBe(false);
        result.Errors.Single()
            .ShouldSatisfyAllConditions(error => error.ErrorMessage.ShouldBe(ResourceErrorMessages.AMOUNT_LESS_THEN_OR_EQUAL_ZERO));
    }
}
