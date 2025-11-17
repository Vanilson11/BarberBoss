using BarberBoss.Communication.Requests;
using Bogus;

namespace CommonTestsUtilities.Requests;
public class RequestUserJsonBuilder
{
    public static RequestUserJson Build()
    {
        return new Faker<RequestUserJson>()
            .RuleFor(user => user.Name, faker => faker.Person.FirstName)
            .RuleFor(user => user.Email, (faker, user) => faker.Internet.Email(user.Name))
            .RuleFor(user => user.Password, faker => faker.Internet.Password(prefix: "!Aa1"));
    }
}
