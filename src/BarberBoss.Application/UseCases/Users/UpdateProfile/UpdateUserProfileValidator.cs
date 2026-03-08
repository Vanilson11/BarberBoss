using BarberBoss.Communication.Requests;
using BarberBoss.Exception;
using FluentValidation;

namespace BarberBoss.Application.UseCases.Users.UpdateProfile;

public class UpdateUserProfileValidator : AbstractValidator<RequestUpdateUserProfileJson>
{
    public UpdateUserProfileValidator()
    {
        RuleFor(request => request.Name).NotEmpty().WithMessage(ResourceErrorMessages.NAME_EMPTY)
            .MinimumLength(2).WithMessage(ResourceErrorMessages.NAME_MIN_LENGTH)
            .When(request => string.IsNullOrWhiteSpace(request.Name) == false, ApplyConditionTo.CurrentValidator)
            .MaximumLength(100).WithMessage(ResourceErrorMessages.NAME_MAX_LENGTH);
        RuleFor(request => request.Email).NotEmpty().WithMessage(ResourceErrorMessages.EMAIL_EMPTY)
            .EmailAddress().WithMessage(ResourceErrorMessages.EMAIL_INVALID)
            .When(request => string.IsNullOrWhiteSpace(request.Email) == false, ApplyConditionTo.CurrentValidator);
        RuleFor(request => request.UpdatedAt).LessThanOrEqualTo(DateTime.UtcNow.Date).WithMessage(ResourceErrorMessages.UPDATED_DATE_USER_CANNOT_BE_THE_FUTURE);
    }
}
