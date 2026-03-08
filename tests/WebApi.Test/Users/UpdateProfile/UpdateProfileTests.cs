using BarberBoss.Exception;
using CommonTestsUtilities.Requests;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Users.UpdateProfile;

public class UpdateProfileTests : BarberBossClassFixture
{
    private const string METHOD = "api/users";
    private readonly string _token;
    private readonly string _emailUserAdmin;
    public UpdateProfileTests(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.User.GetToken();
        _emailUserAdmin = webApplicationFactory.UserAdmin.GetEmail();
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestUpdateUserProfileJsonBuilder.Build();

        var result = await DoPut(requestUri: METHOD, request: request, token: _token);

        result.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }

    [Theory]
    [ClassData(typeof(CultureInfoInlineDataTest))]
    public async Task Error_Name_Empty(string culture)
    {
        var request = RequestUpdateUserProfileJsonBuilder.Build();
        request.Name = string.Empty;

        var result = await DoPut(requestUri: METHOD, request: request, token: _token, culture: culture);

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
        var request = RequestUpdateUserProfileJsonBuilder.Build();
        request.Email = _emailUserAdmin;

        var result = await DoPut(requestUri: METHOD, request: request, token: _token, culture: culture);

        result.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray()
            .Select(error => error.GetString()).ToList();

        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("EMAIL_ALREADY_REGISTERED", new CultureInfo(culture));

        errors.Single()
            .ShouldSatisfyAllConditions(error => error.ShouldBe(expectedMessage));
    }
}
