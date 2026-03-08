using Shouldly;
using System.Net;
using System.Net.Mime;

namespace WebApi.Test.Revenues.Reports;

public class GenerateReportsRevenuesTests : BarberBossClassFixture
{
    private const string METHOD = "api/reports";
    private readonly DateTime _dateStart;
    private readonly DateTime _dateEnd;
    private readonly string _tokenUser;
    private readonly string _tokenUserAdmin;
    public GenerateReportsRevenuesTests(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _dateStart = webApplicationFactory.Revenue_Admin_Member.GetDateStart();
        _dateEnd = webApplicationFactory.Revenue_Admin_Member.GetDateEnd();
        _tokenUser = webApplicationFactory.User.GetToken();
        _tokenUserAdmin = webApplicationFactory.UserAdmin.GetToken();
    }

    [Fact]
    public async Task Success_Excel()
    {
        var result = await DoGet(requestUri: $"{METHOD}/excel?start={_dateStart:yyyy-MM-dd}&end={_dateEnd:yyyy-MM-dd}", token: _tokenUserAdmin);

        result.StatusCode.ShouldBe(HttpStatusCode.OK);

        result.Content.Headers.ContentType.ShouldNotBeNull();
        result.Content.Headers.ContentType.MediaType.ShouldBe(MediaTypeNames.Application.Octet);
    }

    [Fact]
    public async Task Success_Pdf()
    {
        var result = await DoGet(requestUri: $"{METHOD}/pdf?start={_dateStart:yyyy-MM-dd}&end={_dateEnd:yyyy-MM-dd}", token: _tokenUserAdmin);

        result.StatusCode.ShouldBe(HttpStatusCode.OK);

        result.Content.Headers.ContentType.ShouldNotBeNull();
        result.Content.Headers.ContentType.MediaType.ShouldBe(MediaTypeNames.Application.Pdf);
    }

    [Fact]
    public async Task Error_Forbiden_User_Not_Allowed_Excel()
    {
        var result = await DoGet(requestUri: $"{METHOD}/excel?start={_dateStart:yyyy-MM-dd}&end={_dateEnd:yyyy-MM-dd}", token: _tokenUser);

        result.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Error_Forbiden_User_Not_Allowed_Pdf()
    {
        var result = await DoGet(requestUri: $"{METHOD}/pdf?start={_dateStart:yyyy-MM-dd}&end={_dateEnd:yyyy-MM-dd}", token: _tokenUser);

        result.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
}
