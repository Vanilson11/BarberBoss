using BarberBoss.Domain.Entities;
using BarberBoss.Domain.Enums;
using Bogus;

namespace CommonTestsUtilities.Entities;

public class RevenueBuilder
{
    public static List<Revenue> Collection(User user, uint count = 2)
    {
        var list = new List<Revenue>();

        if (count == 0) count = 1;

        var revenueId = 1;

        for(int i = 0; i < count; i++)
        {
            var revenue = Build(user);

            revenue.Id = revenueId++;

            list.Add(revenue);
        }

        return list;
    }

    public static Revenue Build(User user)
    {
        return new Faker<Revenue>()
            .RuleFor(revenue => revenue.Id, _ => 1)
            .RuleFor(revenue => revenue.Title, faker => faker.Commerce.Product())
            .RuleFor(revenue => revenue.Date, faker => faker.Date.Past())
            .RuleFor(revenue => revenue.PaymentType, faker => faker.PickRandom<PaymentType>())
            .RuleFor(revenue => revenue.Amount, faker => faker.Random.Decimal(min: 1, max: 1000))
            .RuleFor(revenue => revenue.UserId, _ => user.Id);
    }
}
