using BarberBoss.Domain.Entities;
using BarberBoss.Exception;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Revenues.GetById;

public class GetRevenueByIdTests : BarberBossClassFixture
{
    private const string METHOD = "api/revenues";
    private readonly string _token;
    private readonly Revenue _revenue;
    public GetRevenueByIdTests(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.User.GetToken();
        _revenue = webApplicationFactory.Revenue_User_Member.GetRevenue();
    }

    [Fact]
    public async Task Success()
    {
        var result = await DoGet(requestUri: $"{METHOD}/{_revenue.Id}", token: _token);

        result.StatusCode.ShouldBe(HttpStatusCode.OK);

        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        response.RootElement.GetProperty("id").GetInt64().ShouldBe(_revenue.Id);
        response.RootElement.GetProperty("title").GetString().ShouldBe(_revenue.Title);
        response.RootElement.GetProperty("date").GetDateTime().ShouldBe(_revenue.Date);
        response.RootElement.GetProperty("paymentType").GetInt32()
            .Equals((BarberBoss.Communication.Enums.PaymentType)_revenue.PaymentType);
        response.RootElement.GetProperty("amount").GetDecimal().Equals(_revenue.Amount);
    }

    [Theory]
    [ClassData(typeof(CultureInfoInlineDataTest))]
    public async Task Error_Revenue_Not_Found(string culture)
    {
        var result = await DoGet(requestUri: $"{METHOD}/1000", token: _token, culture: culture);

        result.StatusCode.ShouldBe(HttpStatusCode.NotFound);

        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray()
            .Select(error => error.GetString()).ToList();

        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("REVENUE_NOT_FOUND", new CultureInfo(culture));

        errors.Single()
            .ShouldSatisfyAllConditions(error => error.ShouldBe(expectedMessage));
    }
}
