using BarberBoss.Communication.Requests;
using BarberBoss.Domain.Enums;
using Bogus;

namespace CommonTestsUtilities.Requests;

public class RequestUpdateUserJsonBuilder
{
    public static RequestUpdateUserJson Build(string? role = Roles.USER)
    {
        return new Faker<RequestUpdateUserJson>()
            .RuleFor(request => request.Name, faker => faker.Person.FirstName)
            .RuleFor(request => request.Email, (faker, request) => faker.Internet.Email(request.Name))
            .RuleFor(request => request.Role, _ => role)
            .RuleFor(request => request.CreatedAt, faker => faker.Date.Past())
            .RuleFor(request => request.UpdatedAt, (_, request) => request.CreatedAt.AddDays(2));
    }
}
