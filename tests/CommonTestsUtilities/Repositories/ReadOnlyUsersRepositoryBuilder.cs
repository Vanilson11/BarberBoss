using BarberBoss.Domain.Entities;
using BarberBoss.Domain.Repositories;
using Moq;

namespace CommonTestsUtilities.Repositories;
public class ReadOnlyUsersRepositoryBuilder
{
    private readonly Mock<IReadOnlyUsersRepository> _mock;

    public ReadOnlyUsersRepositoryBuilder()
    {
        _mock = new Mock<IReadOnlyUsersRepository>();
    }

    public ReadOnlyUsersRepositoryBuilder GetById(User? user)
    {
        if(user is not null)
        {
            _mock.Setup(readOnlyRepository => readOnlyRepository.GetById(user.Id)).ReturnsAsync(user);
        }

        return this;
    }

    public ReadOnlyUsersRepositoryBuilder EmailExists(string? email)
    {
        if(string.IsNullOrWhiteSpace(email) == false)
        {
            _mock.Setup(readOnlyRepository => readOnlyRepository.EmailExists(email)).ReturnsAsync(true);
        }

        return this;
    }

    public ReadOnlyUsersRepositoryBuilder GetUserByEmail(User user)
    {
        if(user != null)
        {
            _mock.Setup(readOnlyRepository => readOnlyRepository.GetUserByEmail(user.Email)).ReturnsAsync(user);
        }

        return this;
    }

    public IReadOnlyUsersRepository Build() => _mock.Object;
}
