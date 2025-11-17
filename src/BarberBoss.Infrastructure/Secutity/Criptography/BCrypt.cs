using BarberBoss.Domain.Security.Criptography;
using BC = BCrypt.Net.BCrypt;

namespace BarberBoss.Infrastructure.Secutity.Criptography;
internal class BCrypt : IPasswordEncripter
{
    public string Encrypt(string password)
    {
        var hashPassword = BC.HashPassword(password);

        return hashPassword;
    }

    public bool Verify(string password, string passwordHash) => BC.Verify(password, passwordHash);
}
