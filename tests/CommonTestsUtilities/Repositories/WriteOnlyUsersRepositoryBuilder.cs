using BarberBoss.Domain.Entities;
using BarberBoss.Domain.Repositories;
using Moq;

namespace CommonTestsUtilities.Repositories;
public class WriteOnlyUsersRepositoryBuilder
{
    private readonly Mock<IWriteOnlyUsersRepository> _mock;

    public WriteOnlyUsersRepositoryBuilder()
    {
        _mock = new Mock<IWriteOnlyUsersRepository>();
    }

    public WriteOnlyUsersRepositoryBuilder Delete(User? user)
    {
        if(user is not null)
        {
            _mock.Setup(repository => repository.Delete(user.Id)).ReturnsAsync(true);
        }

        return this;
    }
    public IWriteOnlyUsersRepository Build()
    {
        return _mock.Object;
    }
}
