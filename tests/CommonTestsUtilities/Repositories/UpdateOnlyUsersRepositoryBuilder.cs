using BarberBoss.Domain.Entities;
using BarberBoss.Domain.Repositories;
using Moq;

namespace CommonTestsUtilities.Repositories;

public class UpdateOnlyUsersRepositoryBuilder
{
    private readonly Mock<IUpdateOnlyUsersRepository> _mock;

    public UpdateOnlyUsersRepositoryBuilder()
    {
        _mock = new Mock<IUpdateOnlyUsersRepository>();
    }

    public UpdateOnlyUsersRepositoryBuilder GetById(User user)
    {
        _mock.Setup(repository => repository.GetById(user.Id)).ReturnsAsync(user);

        return this;
    }

    public IUpdateOnlyUsersRepository Build() { return _mock.Object; }
}
