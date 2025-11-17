using BarberBoss.Application.UseCases.DoLogin;
using BarberBoss.Communication.Requests;
using BarberBoss.Communication.Responses;
using Microsoft.AspNetCore.Mvc;

namespace BarberBoss.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LoginController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisterUserJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorMessagesJson), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> DoLogin(
        [FromServices] IDoLoginUseCase useCase,
        [FromBody] RequestDoLoginJson request)
    {
        var response = await useCase.Execute(request);

        return Ok(response);
    }
}
