using BarberBoss.Communication.Requests;
using FluentValidation;

namespace BarberBoss.Application.UseCases.Users;
public class UserUseCaseValidator : AbstractValidator<RequestUserJson>
{
    public UserUseCaseValidator()
    {
        RuleFor(user => user.Name).NotEmpty().WithMessage("Name is required.");
        RuleFor(user => user.Email)
            .NotEmpty().WithMessage("E-mail is required.")
            .EmailAddress().WithMessage("E-mail invalid.");
        RuleFor(user => user.Password).SetValidator(new PasswordValidator<RequestUserJson>());
    }
}
