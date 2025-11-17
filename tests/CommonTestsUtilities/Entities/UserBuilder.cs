using BarberBoss.Domain.Entities;
using Bogus;
using CommonTestsUtilities.Security.Criptography;

namespace CommonTestsUtilities.Entities;
public class UserBuilder
{
    public static User Build()
    {
        var passwordEncrypter = new PasswordEncrypterBuilder().Build();
        return new Faker<User>()
            .RuleFor(user => user.Id, _ => 1)
            .RuleFor(user => user.Name, faker => faker.Person.FirstName)
            .RuleFor(user => user.Email, (faker, user) => faker.Internet.Email(user.Name))
            .RuleFor(user => user.Password, (_, user) => passwordEncrypter.Encrypt(user.Password))
            .RuleFor(user => user.UserIdentifier, _ => Guid.NewGuid());
    }
}
