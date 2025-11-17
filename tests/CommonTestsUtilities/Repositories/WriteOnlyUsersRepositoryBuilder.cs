using BarberBoss.Domain.Repositories;
using Moq;

namespace CommonTestsUtilities.Repositories;
public class WriteOnlyUsersRepositoryBuilder
{
    public static IWriteOnlyUsersRepository Build()
    {
        var mock = new Mock<IWriteOnlyUsersRepository>();

        return mock.Object;
    }
}
