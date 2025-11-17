using BarberBoss.Communication.Requests;
using Bogus;

namespace CommonTestsUtilities.Requests;
public class RequestLoginUseCaseBuilder
{
    public static RequestDoLoginJson Build()
    {
        return new Faker<RequestDoLoginJson>()
            .RuleFor(user => user.Email, (faker, user) => faker.Internet.Email(user.Email))
            .RuleFor(user => user.Password, faker => faker.Internet.Password(prefix: "!Aa1"));
    }
}
