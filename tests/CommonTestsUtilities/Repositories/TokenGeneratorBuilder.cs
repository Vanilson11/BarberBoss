using BarberBoss.Domain.Entities;
using BarberBoss.Domain.Security.Tokens;
using Moq;

namespace CommonTestsUtilities.Repositories;
public class TokenGeneratorBuilder
{
    public static IAccessTokenGenerator Build()
    {
        var mock = new Mock<IAccessTokenGenerator>();

        mock.Setup(tokenGenerator => tokenGenerator.Generate(It.IsAny<User>())).Returns("token_test");

        return mock.Object;
    }
}
