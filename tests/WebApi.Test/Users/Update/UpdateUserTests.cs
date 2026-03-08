using BarberBoss.Domain.Entities;
using BarberBoss.Exception;
using CommonTestsUtilities.Requests;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Users.Update;

public class UpdateUserTests : BarberBossClassFixture
{
    private const string METHOD = "api/users";
    private readonly string _token;
    private readonly string _tokenAdmin;
    private readonly string _emailAdmin;
    private readonly User _user;
    public UpdateUserTests(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.User.GetToken();
        _tokenAdmin = webApplicationFactory.UserAdmin.GetToken();
        _emailAdmin = webApplicationFactory.UserAdmin.GetEmail();
        _user = webApplicationFactory.User.GetUser();
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestUpdateUserJsonBuilder.Build();

        var result = await DoPut(requestUri: $"{METHOD}/{_user.Id}", request: request, token: _tokenAdmin);

        result.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }

    [Theory]
    [ClassData(typeof(CultureInfoInlineDataTest))]
    public async Task Error_User_Not_Found(string culture)
    {
        var request = RequestUpdateUserJsonBuilder.Build();

        var result = await DoPut(requestUri: $"{METHOD}/1000", request: request, token: _tokenAdmin, culture: culture);

        result.StatusCode.ShouldBe(HttpStatusCode.NotFound);

        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray()
            .Select(error => error.GetString()).ToList();

        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("USER_NOT_FOUND", new CultureInfo(culture));

        errors.Single()
            .ShouldSatisfyAllConditions(error => error.ShouldBe(expectedMessage));
    }

    [Theory]
    [ClassData(typeof(CultureInfoInlineDataTest))]
    public async Task Error_User_Name_Empty(string culture)
    {
        var request = RequestUpdateUserJsonBuilder.Build();
        request.Name = string.Empty;

        var result = await DoPut(requestUri: $"{METHOD}/{_user.Id}", request: request, token: _tokenAdmin, culture: culture);

        result.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray()
            .Select(error => error.GetString()).ToList();

        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("NAME_EMPTY", new CultureInfo(culture));

        errors.Single()
            .ShouldSatisfyAllConditions(error => error.ShouldBe(expectedMessage));
    }

    [Theory]
    [ClassData(typeof(CultureInfoInlineDataTest))]
    public async Task Error_Email_Exists(string culture)
    {
        var request = RequestUpdateUserJsonBuilder.Build();
        request.Email = _emailAdmin;

        var result = await DoPut(requestUri: $"{METHOD}/{_user.Id}", request: request, token: _tokenAdmin, culture: culture);

        result.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray()
            .Select(error => error.GetString()).ToList();

        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("EMAIL_ALREADY_REGISTERED", new CultureInfo(culture));

        errors.Single()
            .ShouldSatisfyAllConditions(error => error.ShouldBe(expectedMessage));
    }

    [Fact]
    public async Task Error_Forbiden_User_Not_Allowed()
    {
        var request = RequestUpdateUserJsonBuilder.Build();

        var result = await DoPut(requestUri: $"{METHOD}/{_user.Id}", request: request, token: _token);

        result.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
}
