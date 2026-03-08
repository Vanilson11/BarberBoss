using BarberBoss.Domain.Entities;
using BarberBoss.Domain.Services.LoggedUser;
using Moq;

namespace CommonTestsUtilities.Services.LoggedUser;

public class LoggedUserBuilder
{
    public static ILoggedUser Build(User user)
    {
        var mock = new Mock<ILoggedUser>();

        if(user is not null)
        {
            mock.Setup(loggedUser => loggedUser.Get()).ReturnsAsync(user);
        }

        return mock.Object;
    }
}
