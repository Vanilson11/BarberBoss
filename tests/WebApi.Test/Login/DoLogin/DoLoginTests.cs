using BarberBoss.Communication.Requests;
using BarberBoss.Exception;
using CommonTestsUtilities.Requests;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Login.DoLogin;
public class DoLoginTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _httpClient;
    private const string METHOD = "api/login";
    private readonly string _email;
    private readonly string _password;
    private readonly string _name;

    public DoLoginTests(CustomWebApplicationFactory webApplicationFactory)
    {
        _httpClient = webApplicationFactory.CreateClient();
        _email = webApplicationFactory.GetEmail();
        _password = webApplicationFactory.GetPassword();
        _name = webApplicationFactory.GetName();
    }

    [Fact]
    public async Task Success()
    {
        var request = new RequestDoLoginJson()
        {
            Email = _email,
            Password = _password
        };

        var result = await _httpClient.PostAsJsonAsync(METHOD, request);

        result.StatusCode.ShouldBe(HttpStatusCode.OK);

        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        response.RootElement.GetProperty("name").GetString().ShouldBe(_name);
        response.RootElement.GetProperty("token").GetString().ShouldNotBeNullOrWhiteSpace();
    }

    [Theory]
    [ClassData(typeof(CultureInfoInlineDataTest))]
    public async Task Error_Email_Or_Password_Invalid(string cultureInfo)
    {
        var request = RequestLoginUseCaseBuilder.Build();
        request.Email = _email;

        _httpClient.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue(cultureInfo));

        var result = await _httpClient.PostAsJsonAsync(METHOD, request);

        result.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);

        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray();

        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("EMAIL_OR_PASSWORD_INVALID", new CultureInfo(cultureInfo));

        errors.Single().ShouldSatisfyAllConditions(error => error.GetString().ShouldBe(expectedMessage));
    }
}
