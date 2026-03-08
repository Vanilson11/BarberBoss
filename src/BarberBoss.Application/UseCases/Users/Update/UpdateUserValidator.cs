using BarberBoss.Communication.Requests;
using BarberBoss.Domain.Enums;
using BarberBoss.Exception;
using FluentValidation;

namespace BarberBoss.Application.UseCases.Users.Update;

public class UpdateUserValidator : AbstractValidator<RequestUpdateUserJson>
{
    public UpdateUserValidator()
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
        RuleFor(user => user.Role).NotEmpty().WithMessage(ResourceErrorMessages.ROLE_EMPTY)
            .Must(role => role == Roles.ADMIN || role == Roles.USER).WithMessage(ResourceErrorMessages.ROLE_INVALID)
            .When(user => string.IsNullOrWhiteSpace(user.Role) == false, ApplyConditionTo.CurrentValidator);
        RuleFor(user => user.CreatedAt).LessThanOrEqualTo(DateTime.UtcNow).WithMessage(ResourceErrorMessages.CREATED_AT_CANNOT_BE_THE_FUTURE);
    }
}
