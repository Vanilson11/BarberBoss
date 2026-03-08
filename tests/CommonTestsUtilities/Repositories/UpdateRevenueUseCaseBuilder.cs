using BarberBoss.Domain.Entities;
using BarberBoss.Domain.Repositories;
using Moq;

namespace CommonTestsUtilities.Repositories;

public class UpdateRevenueUseCaseBuilder
{
    private readonly Mock<IUpdateOnlyRevenueRepository> _mock;

    public UpdateRevenueUseCaseBuilder()
    {
        _mock = new Mock<IUpdateOnlyRevenueRepository>();
    }

    public UpdateRevenueUseCaseBuilder GetById(User user, Revenue? revenue)
    {
        if (revenue is not null)
        {
            _mock.Setup(repository => repository.GetById(user, It.IsAny<long>())).ReturnsAsync(revenue);
        }

        return this;
    }

    public IUpdateOnlyRevenueRepository Build() { return _mock.Object; }
}
