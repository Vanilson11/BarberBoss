
using System.Net;

namespace BarberBoss.Exception.ExceptionsBase;
public class InvalidLoginException : BarberBossException
{
    public InvalidLoginException(string message) : base(message)
    {
    }

    public override int StatusCode => (int)HttpStatusCode.Unauthorized;

    public override List<string> GetErrors()
    {
        return [Message];
    }
}
