using BarberBoss.Domain.Entities;
using BarberBoss.Domain.Security.Criptography;
using BarberBoss.Infrastructure.DataAccess;
using CommonTestsUtilities.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi.Test;
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private User _user;
    private string _password;
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

                StartDatabase(dbContext, passwordEncrypter);
            });
    }

    public string GetEmail() => _user.Email;
    public string GetPassword() => _password;

    public string GetName() => _user.Name;

    private void StartDatabase(BarberBossDbContext context, IPasswordEncripter passwordEncripter)
    {
        _user = UserBuilder.Build();

        _password = _user.Password;

        _user.Password = passwordEncripter.Encrypt(_user.Password);

        context.Users.Add(_user);

        context.SaveChanges();
    }
}
