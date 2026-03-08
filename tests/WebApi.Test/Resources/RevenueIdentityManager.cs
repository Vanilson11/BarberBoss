using BarberBoss.Domain.Entities;

namespace WebApi.Test.Resources;

public class RevenueIdentityManager
{
    private readonly Revenue _revenue;
    public RevenueIdentityManager(Revenue revenue)
    {
        _revenue = revenue;
    }

    public long GetId() => _revenue.Id;
    public DateTime GetDateStart() => _revenue.Date;
    public DateTime GetDateEnd() => _revenue.Date.AddDays(2);
    public Revenue GetRevenue() => _revenue;
}
