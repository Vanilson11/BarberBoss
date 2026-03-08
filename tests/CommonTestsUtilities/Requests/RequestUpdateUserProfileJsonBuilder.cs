using BarberBoss.Communication.Requests;
using Bogus;

namespace CommonTestsUtilities.Requests;

public class RequestUpdateUserProfileJsonBuilder
{
    public static RequestUpdateUserProfileJson Build()
    {
        return new Faker<RequestUpdateUserProfileJson>()
            .RuleFor(request => request.Name, faker => faker.Person.FirstName)
            .RuleFor(request => request.Email, (faker, request) => faker.Internet.Email(request.Name))
            .RuleFor(request => request.UpdatedAt, faker => faker.Date.Past());
    }
}
