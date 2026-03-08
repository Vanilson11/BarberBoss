using BarberBoss.Communication.Requests;
using BarberBoss.Exception;
using CommonTestsUtilities.Requests;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Users.ChangePassword;

public class ChangePasswordTests : BarberBossClassFixture
{
    private const string METHOD = "api/users";
    private readonly string _token;
    private readonly string _password;
    private readonly string _email;
    public ChangePasswordTests(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.User.GetToken();
        _password = webApplicationFactory.User.GetPassword();
        _email = webApplicationFactory.User.GetEmail();
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestChangePasswordJsonBuilder.Build();
        request.Password = _password;

        var result = await DoPut(requestUri: $"{METHOD}/change-password", request: request, token: _token);

        result.StatusCode.ShouldBe(HttpStatusCode.NoContent);

        var logginRequest = new RequestDoLoginJson()
        {
            Email = _email,
            Password = _password,
        };

        result = await DoPost(requestUri: "api/login", request: logginRequest);
        result.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);

        logginRequest.Password = request.NewPassword;

        result = await DoPost(requestUri: "api/login", request: logginRequest);
        result.StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    [Theory]
    [ClassData(typeof(CultureInfoInlineDataTest))]
    public async Task Error_Current_Password_Different(string culture)
    {
        var request = RequestChangePasswordJsonBuilder.Build();

        var result = await DoPut(requestUri: $"{METHOD}/change-password", request: request, token: _token, culture: culture);

        result.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray()
            .Select(error => error.GetString()).ToList();

        var expectedMessage = ResourceErrorMessages.ResourceManager
            .GetString("PASSWORD_DIFFERENT_CURRENT_PASSWORD", new CultureInfo(culture));

        errors.Single()
            .ShouldSatisfyAllConditions(error => error.ShouldBe(expectedMessage));
    }
}
