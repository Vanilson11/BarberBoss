using Microsoft.Extensions.Configuration;

namespace BarberBoss.Infrastructure;
public static class ConfigurationExtentions
{
    public static bool IsTestEnvironment(this IConfiguration configuration)
    {
        return configuration.GetValue<bool>("InMemoryTest");
    }
}
