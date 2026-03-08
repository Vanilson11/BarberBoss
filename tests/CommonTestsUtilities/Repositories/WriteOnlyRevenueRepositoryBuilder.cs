using BarberBoss.Domain.Repositories;
using Moq;

namespace CommonTestsUtilities.Repositories;

public class WriteOnlyRevenueRepositoryBuilder
{
    public static IWriteOnlyRevenueRepository Build()
    {
        var mock = new Mock<IWriteOnlyRevenueRepository>();

        return mock.Object;
    }
}
