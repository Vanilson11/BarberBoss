using BarberBoss.Communication.Enums;
using BarberBoss.Communication.Requests;
using Bogus;

namespace CommonTestsUtilities;
public class RequestRevenuesJsonBuilder
{
    public RequestRevenuesJson Build()
    {
        return new Faker<RequestRevenuesJson>()
            .RuleFor(revenue => revenue.Title, faker => faker.Commerce.ProductName())
            .RuleFor(revenue => revenue.Date, faker => faker.Date.Past())
            .RuleFor(revenue => revenue.PaymentType, faker => faker.PickRandom<PaymentType>())
            .RuleFor(revenue => revenue.Amount, faker => faker.Random.Decimal(min: 1, max: 9999))
            .RuleFor(revenue => revenue.Description, faker => faker.Lorem.Random.Word());
    }
}
