using BarberBoss.Domain.Entities;
using BarberBoss.Domain.Repositories;
using Moq;

namespace CommonTestsUtilities.Repositories;

public class ReportsReadOnlyRepositoryBuilder
{
    private readonly Mock<IReportsReadOnlyRepository> _mock;

    public ReportsReadOnlyRepositoryBuilder()
    {
        _mock = new Mock<IReportsReadOnlyRepository>();
    }

    public ReportsReadOnlyRepositoryBuilder GetByWeek(User user, List<Revenue> revenues)
    {
        _mock.Setup(repository => repository.GetByWeek(user, It.IsAny<DateOnly>(), It.IsAny<DateOnly>())).ReturnsAsync(revenues);

        return this;
    }

    public IReportsReadOnlyRepository Build() { return _mock.Object; }
}
