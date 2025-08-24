using BarberBoss.Domain.Enums;

namespace BarberBoss.Domain.Entities;
public class Revenue
{
    public long Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public PaymentType PaymentType { get; set; }
    public decimal Amount { get; set; }
    public string? Description { get; set; }
}
