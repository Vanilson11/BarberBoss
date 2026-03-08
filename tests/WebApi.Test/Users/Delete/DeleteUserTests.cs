using BarberBoss.Communication.Requests;
using BarberBoss.Exception;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Users.Delete;

public class DeleteUserTests : BarberBossClassFixture
{
    private const string METHOD = "api/users";
    private readonly string _tokenAdmin;
    private readonly string _tokenUser;
    private readonly string _emailUser;
    private readonly string _passwordUser;
    private readonly long _userId;
    public DeleteUserTests(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _tokenAdmin = webApplicationFactory.UserAdmin.GetToken();
        _tokenUser = webApplicationFactory.User.GetToken();
        _emailUser = webApplicationFactory.User.GetEmail();
        _passwordUser = webApplicationFactory.User.GetPassword();
        _userId = webApplicationFactory.User.GetId();
    }

    [Fact]
    public async Task Success()
    {
        var result = await DoDelte(requestUri: $"{METHOD}/{_userId}", token: _tokenAdmin);

        result.StatusCode.ShouldBe(HttpStatusCode.NoContent);

        var loginRequest = new RequestDoLoginJson()
        {
            Email = _emailUser,
            Password = _passwordUser,
        };

        result = await DoPost(requestUri: "api/login", request: loginRequest);
        result.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Theory]
    [ClassData(typeof(CultureInfoInlineDataTest))]
    public async Task Error_User_Not_Found(string culture)
    {
        var result = await DoDelte(requestUri: $"{METHOD}/1000", token: _tokenAdmin, culture: culture);

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
    public async Task Error_Forbiden_User_Not_Allowed()
    {
        var result = await DoDelte(requestUri: $"{METHOD}/{_userId}", token: _tokenUser);

        result.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
}
