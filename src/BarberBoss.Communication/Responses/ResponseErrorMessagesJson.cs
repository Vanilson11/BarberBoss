namespace BarberBoss.Communication.Responses;
public class ResponseErrorMessagesJson
{
    public List<string> ErrorMessages { get; set; }
    public ResponseErrorMessagesJson(List<string> errorMessages)
    {
        ErrorMessages = errorMessages;
    }

    public ResponseErrorMessagesJson(string message)
    {
        ErrorMessages = [message];
    }
}
