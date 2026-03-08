using BarberBoss.Communication.Requests;
using Bogus;

namespace CommonTestsUtilities.Requests;
public class RequestUserJsonBuilder
{
    public static RequestRegisterUserJson Build()
    {
        return new Faker<RequestRegisterUserJson>()
            .RuleFor(user => user.Name, faker => faker.Person.FirstName)
            .RuleFor(user => user.Email, (faker, user) => faker.Internet.Email(user.Name))
            .RuleFor(user => user.Password, faker => faker.Internet.Password(prefix: "!Aa1"))
            .RuleFor(user => user.CreatedAt, faker => faker.Date.Past());
    }
}
