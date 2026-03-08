using BarberBoss.Domain.Entities;
using BarberBoss.Exception;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Users.GetById;

public class GetUserByIdTests : BarberBossClassFixture
{
    private const string METHOD = "api/users";
    private readonly string _tokenAdmin;
    private readonly string _tokenUser;
    private readonly User _user;
    public GetUserByIdTests(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _tokenAdmin = webApplicationFactory.UserAdmin.GetToken();
        _tokenUser = webApplicationFactory.User.GetToken();
        _user = webApplicationFactory.User.GetUser();
    }

    [Fact]
    public async Task Success()
    {
        var result = await DoGet(requestUri: $"{METHOD}/{_user.Id}", token: _tokenAdmin);

        result.StatusCode.ShouldBe(HttpStatusCode.OK);

        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        response.RootElement.GetProperty("name").GetString().ShouldBe(_user.Name);
        response.RootElement.GetProperty("email").GetString().ShouldBe(_user.Email);
        response.RootElement.GetProperty("role").GetString().ShouldBe(_user.Role);
        response.RootElement.GetProperty("createdAt").GetDateTime().ShouldBe(_user.CreatedAt);
        response.RootElement.GetProperty("updatedAt").GetDateTime().ShouldBe(_user.UpdatedAt);
    }

    [Theory]
    [ClassData(typeof(CultureInfoInlineDataTest))]
    public async Task Error_User_Not_Found(string culture)
    {
        var result = await DoGet(requestUri: $"{METHOD}/1000", token: _tokenAdmin, culture: culture);

        result.StatusCode.ShouldBe(HttpStatusCode.NotFound);

        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray()
            .Select(error => error.GetString()).ToList();

        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("USER_NOT_FOUND", new CultureInfo(culture));

        errors.Single()
            .ShouldSatisfyAllConditions(error => error.ShouldBe(expectedMessage));
    }

    [Fact]
    public async Task Error_Forbidden_User_Not_Allowed()
    {
        var result = await DoGet(requestUri: $"{METHOD}/{_user.Id}", token: _tokenUser);

        result.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
}
