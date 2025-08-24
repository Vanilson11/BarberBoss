using System.Net;

namespace BarberBoss.Exception.ExceptionsBase;
public class ErrorsOnValidationException : BarberBossException
{
    private List<string> _errors;

    public ErrorsOnValidationException(List<string> errorMessages) : base(string.Empty)
    {
        _errors = errorMessages;
    }

    public override int StatusCode => (int)HttpStatusCode.BadRequest;

    public override List<string> GetErrors()
    {
        return _errors;
    }
}
