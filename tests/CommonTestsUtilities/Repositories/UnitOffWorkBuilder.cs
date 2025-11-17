using BarberBoss.Domain.Repositories;
using Moq;

namespace CommonTestsUtilities.Repositories;
public class UnitOffWorkBuilder
{
    public static IUnitOffWork Build()
    {
        var mock = new Mock<IUnitOffWork>();

        return mock.Object;
    }
}
