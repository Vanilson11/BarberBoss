using BarberBoss.Communication.Enums;

namespace BarberBoss.Communication.Requests;
public class RequestRevenuesJson
{
    public string Title { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public PaymentType PaymentType { get; set; }
    public decimal Amount { get; set; }
    public string? Description { get; set; }
}
