using BarberBoss.Communication.Requests;
using BarberBoss.Exception;
using FluentValidation;

namespace BarberBoss.Application.UseCases.Users;
public class UserUseCaseValidator : AbstractValidator<RequestRegisterUserJson>
{
    public UserUseCaseValidator()
    {
        RuleFor(user => user.Name).NotEmpty().WithMessage(ResourceErrorMessages.NAME_EMPTY)
            .MinimumLength(2).WithMessage(ResourceErrorMessages.NAME_MIN_LENGTH)
            .When(user => string.IsNullOrWhiteSpace(user.Name) == false, ApplyConditionTo.CurrentValidator)
            .MaximumLength(100).WithMessage(ResourceErrorMessages.NAME_MAX_LENGTH);
        RuleFor(user => user.Email)
            .NotEmpty().WithMessage(ResourceErrorMessages.EMAIL_EMPTY)
            .EmailAddress()
            .When(user => string.IsNullOrWhiteSpace(user.Email) == false, ApplyConditionTo.CurrentValidator)
            .WithMessage(ResourceErrorMessages.EMAIL_INVALID);
        RuleFor(user => user.CreatedAt).LessThanOrEqualTo(DateTime.UtcNow).WithMessage(ResourceErrorMessages.CREATED_AT_CANNOT_BE_THE_FUTURE);
        RuleFor(user => user.Password).SetValidator(new PasswordValidator<RequestRegisterUserJson>());
    }
}
