using BarberBoss.Exception;
using CommonTestsUtilities.Requests;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Revenues.Update;

public class UpdateRevenueTests : BarberBossClassFixture
{
    private const string METHOD = "api/revenues";
    private readonly string _token;
    private readonly long _revenueId;
    public UpdateRevenueTests(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.User.GetToken();
        _revenueId = webApplicationFactory.Revenue_User_Member.GetId();
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestRevenuesJsonBuilder.Build();

        var result = await DoPut(requestUri: $"{METHOD}/{_revenueId}", request: request, token: _token);

        result.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }

    [Theory]
    [ClassData(typeof(CultureInfoInlineDataTest))]
    public async Task Error_Title_Empty(string culture)
    {
        var request = RequestRevenuesJsonBuilder.Build();
        request.Title = string.Empty;

        var result = await DoPut(requestUri: $"{METHOD}/{_revenueId}", request: request, token: _token, culture: culture);

        result.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray()
            .Select(error => error.GetString()).ToList();

        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("TITLE_REQUIRED", new CultureInfo(culture));

        errors.Single()
            .ShouldSatisfyAllConditions(error => error.ShouldBe(expectedMessage));
    }

    [Theory]
    [ClassData(typeof(CultureInfoInlineDataTest))]
    public async Task Error_Revenue_Not_Found(string culture)
    {
        var request = RequestRevenuesJsonBuilder.Build();

        var result = await DoPut(requestUri: $"{METHOD}/1000", request: request, token: _token, culture: culture);

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
