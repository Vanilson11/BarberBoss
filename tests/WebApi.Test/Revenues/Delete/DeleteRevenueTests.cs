using BarberBoss.Exception;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Revenues.Delete;

public class DeleteRevenueTests : BarberBossClassFixture
{
    private const string METHOD = "api/revenues";
    private readonly string _token;
    private readonly long _revenueId;
    public DeleteRevenueTests(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.User.GetToken();
        _revenueId = webApplicationFactory.Revenue_User_Member.GetId();
    }

    [Fact]
    public async Task Success()
    {
        var result = await DoDelte(requestUri: $"{METHOD}/{_revenueId}", token: _token);

        result.StatusCode.ShouldBe(HttpStatusCode.NoContent);

        result = await DoGet(requestUri: $"{METHOD}/{_revenueId}", token: _token);

        result.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Theory]
    [ClassData(typeof(CultureInfoInlineDataTest))]
    public async Task Error_Revenue_Not_Found(string culture)
    {
        var result = await DoDelte(requestUri: $"{METHOD}/1000", token: _token, culture: culture);

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
