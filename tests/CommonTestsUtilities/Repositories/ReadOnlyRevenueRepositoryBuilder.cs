using BarberBoss.Domain.Entities;
using BarberBoss.Domain.Repositories;
using Moq;

namespace CommonTestsUtilities.Repositories;

public class ReadOnlyRevenueRepositoryBuilder
{
    private readonly Mock<IReadOnlyRevenueRepository> _mock;

    public ReadOnlyRevenueRepositoryBuilder()
    {
        _mock = new Mock<IReadOnlyRevenueRepository>();
    }

    public ReadOnlyRevenueRepositoryBuilder GetAllRevenues(User user, List<Revenue> revenues)
    {
        if(user is not null)
        {
            _mock.Setup(repository => repository.GetAllRevenues(user)).ReturnsAsync(revenues);
        }

        return this;
    }

    public ReadOnlyRevenueRepositoryBuilder GetById(User user, Revenue? revenue)
    {
        if(revenue is not null)
        {
            _mock.Setup(repository => repository.GetById(user, It.IsAny<long>())).ReturnsAsync(revenue);
        }

        return this;
    }

    public IReadOnlyRevenueRepository Build() { return _mock.Object; }
}
