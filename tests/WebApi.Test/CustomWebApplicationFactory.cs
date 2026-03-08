using BarberBoss.Domain.Entities;
using BarberBoss.Domain.Enums;
using BarberBoss.Domain.Security.Criptography;
using BarberBoss.Domain.Security.Tokens;
using BarberBoss.Infrastructure.DataAccess;
using CommonTestsUtilities.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Test.Resources;

namespace WebApi.Test;
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    public RevenueIdentityManager Revenue_User_Member { get; private set; } = default!;
    public RevenueIdentityManager Revenue_Admin_Member { get; private set; } = default!;
    public UserIdentityManager UserAdmin { get; private set; } = default!;
    public UserIdentityManager User { get; private set; } = default!;
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Tests")
            .ConfigureServices(service =>
            {
                var serviceProvider = service.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

                service.AddDbContext<BarberBossDbContext>(config =>
                {
                    config.UseInMemoryDatabase("InMemoryDbForTesting");
                    config.UseInternalServiceProvider(serviceProvider);
                });

                var scope = service.BuildServiceProvider().CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<BarberBossDbContext>();
                var passwordEncrypter = scope.ServiceProvider.GetRequiredService<IPasswordEncripter>();
                var tokenGenerator = scope.ServiceProvider.GetRequiredService<IAccessTokenGenerator>();

                StartDatabase(dbContext, passwordEncrypter, tokenGenerator);

            });
    }

    private void StartDatabase(BarberBossDbContext context, IPasswordEncripter passwordEncripter, IAccessTokenGenerator tokenGenerator)
    {
        var user = AddUser(context, passwordEncripter, tokenGenerator);
        var revenueUser = AddRevenue(context, user, revenueId: 1);
        Revenue_User_Member = new RevenueIdentityManager(revenueUser);

        var userAdmin = AddUserAdmin(context, passwordEncripter, tokenGenerator);
        var revenueUserAdmin = AddRevenue(context, userAdmin, revenueId: 2);
        Revenue_Admin_Member = new RevenueIdentityManager(revenueUserAdmin);

        context.SaveChanges();
    }

    private User AddUser(BarberBossDbContext context, IPasswordEncripter passwordEncripter, IAccessTokenGenerator tokenGenerator)
    {
        var user = UserBuilder.Build();
        user.Id = 1;

        var password = user.Password;

        user.Password = passwordEncripter.Encrypt(user.Password);

        context.Users.Add(user);

        var token = tokenGenerator.Generate(user);

        User = new UserIdentityManager(user, password, token);

        return user;
    }

    private User AddUserAdmin(BarberBossDbContext context, IPasswordEncripter passwordEncripter, IAccessTokenGenerator tokenGenerator)
    {
        var user = UserBuilder.Build(Roles.ADMIN);
        user.Id = 2;

        var password = user.Password;

        user.Password = passwordEncripter.Encrypt(user.Password);

        context.Users.Add(user);

        var token = tokenGenerator.Generate(user);

        UserAdmin = new UserIdentityManager(user, password, token);

        return user;
    }

    private Revenue AddRevenue(BarberBossDbContext context, User user, long revenueId)
    {
        var revenue = RevenueBuilder.Build(user);
        revenue.Id = revenueId;
        revenue.Date = revenue.Date.Date;

        context.Revenues.Add(revenue);

        return revenue;
    }
}
