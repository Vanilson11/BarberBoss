using BarberBoss.Communication.Requests;
using BarberBoss.Exception;
using FluentValidation;

namespace BarberBoss.Application.UseCases.Revenues;
public class RevenueUseCaseValidator : AbstractValidator<RequestRevenuesJson>
{
    public RevenueUseCaseValidator()
    {
        RuleFor(revenue => revenue.Title).NotEmpty().WithMessage(ResourceErrorMessages.TITLE_REQUIRED);
        RuleFor(revenue => revenue.Date).LessThanOrEqualTo(DateTime.UtcNow).WithMessage(ResourceErrorMessages.REVENUE_CANNOT_BE_THE_FUTURE);
        RuleFor(revenue => revenue.PaymentType).IsInEnum().WithMessage(ResourceErrorMessages.PAYMENT_TYPE_INVALID);
        RuleFor(revenue => revenue.Amount).GreaterThan(0).WithMessage(ResourceErrorMessages.AMOUNT_LESS_THEN_OR_EQUAL_ZERO);
    }
}
