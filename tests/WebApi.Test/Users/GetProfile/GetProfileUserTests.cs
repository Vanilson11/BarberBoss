using BarberBoss.Domain.Entities;
using Shouldly;
using System.Net;
using System.Text.Json;

namespace WebApi.Test.Users.GetProfile;

public class GetProfileUserTests : BarberBossClassFixture
{
    private const string METHOD = "api/users";
    private readonly string _token;
    private readonly User _user;
    public GetProfileUserTests(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.User.GetToken();
        _user = webApplicationFactory.User.GetUser();
    }

    [Fact]
    public async Task Success()
    {
        var result = await DoGet(requestUri: METHOD, token: _token);

        result.StatusCode.ShouldBe(HttpStatusCode.OK);

        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        response.RootElement.GetProperty("name").GetString().ShouldBe(_user.Name);
        response.RootElement.GetProperty("email").GetString().ShouldBe(_user.Email);
        response.RootElement.GetProperty("role").GetString().ShouldBe(_user.Role);
        response.RootElement.GetProperty("createdAt").GetDateTime().ShouldBe(_user.CreatedAt);
        response.RootElement.GetProperty("updatedAt").GetDateTime().ShouldBe(_user.UpdatedAt);
    }
}
