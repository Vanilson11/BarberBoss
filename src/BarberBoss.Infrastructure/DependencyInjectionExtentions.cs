using BarberBoss.Domain.Repositories;
using BarberBoss.Domain.Security.Criptography;
using BarberBoss.Domain.Security.Tokens;
using BarberBoss.Infrastructure.DataAccess;
using BarberBoss.Infrastructure.DataAccess.Repositories;
using BarberBoss.Infrastructure.Secutity.Tokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BarberBoss.Infrastructure;
public static class DependencyInjectionExtentions
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        AddDbContext(services, configuration);
        AddTokens(services, configuration);
        AddRepositories(services);
    }

    private static void AddDbContext(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Connection");
        var serverVersion = new MySqlServerVersion(new Version(8, 0, 42));

        services.AddDbContext<BarberBossDbContext>(config => config.UseMySql(connectionString, serverVersion));
    }

    private static void AddTokens(this IServiceCollection services, IConfiguration configuration)
    {
        var key = configuration.GetValue<string>("Settings:Jwt:SiginingKey");
        var expiresMinutes = configuration.GetValue<uint>("Settings:Jwt:ExpiresMinutes");

        services.AddScoped<IAccessTokenGenerator>(config => new JwtTokenGenerator(expiresMinutes, key!));
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IUnitOffWork, UnitOffWork>();
        services.AddScoped<IWriteOnlyRevenueRepository, RevenueRepository>();
        services.AddScoped<IReadOnlyRevenueRepository, RevenueRepository>();
        services.AddScoped<IUpdateOnlyRevenueRepository, RevenueRepository>();
        services.AddScoped<IReportsReadOnlyRepository, RevenueRepository>();
        services.AddScoped<IReadOnlyUsersRepository, UsersRepository>();
        services.AddScoped<IWriteOnlyUsersRepository, UsersRepository>();

        services.AddScoped<IPasswordEncripter, Secutity.Criptography.BCrypt>();
    }
}
