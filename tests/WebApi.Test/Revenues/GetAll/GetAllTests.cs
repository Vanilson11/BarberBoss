using Shouldly;
using System.Net;
using System.Text.Json;

namespace WebApi.Test.Revenues.GetAll;

public class GetAllTests : BarberBossClassFixture
{
    private const string METOHD = "api/revenues";
    private readonly string _token;
    public GetAllTests(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.User.GetToken();
    }

    [Fact]
    public async Task Success()
    {
        var result = await DoGet(requestUri: METOHD, token: _token);

        result.StatusCode.ShouldBe(HttpStatusCode.OK);

        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        response.RootElement.GetProperty("revenues").EnumerateArray().ShouldNotBeEmpty();
    }
}
