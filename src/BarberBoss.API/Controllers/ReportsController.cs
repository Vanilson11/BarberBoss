using BarberBoss.Application.UseCases.Revenues.Reports.Excel;
using BarberBoss.Application.UseCases.Revenues.Reports.Pdf;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace BarberBoss.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReportsController : ControllerBase
{
    [HttpGet("excel")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetExcel([FromServices] IReportRevenuesExcelUseCase useCase,
        [FromHeader] DateOnly start,
        [FromHeader] DateOnly end)
    {
        byte[] file = await useCase.Execute(start, end);

        if (file.Length > 0)
            return File(file, MediaTypeNames.Application.Octet, "report.xlsx");

        return NoContent();
    }

    [HttpGet("pdf")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetPdf([FromServices] IReportRevenuePdfUseCase useCase,
        [FromHeader] DateOnly start,
        [FromHeader] DateOnly end)
    {
        byte[] file = await useCase.Execute(start, end);

        if (file.Length > 0)
            return File(file, MediaTypeNames.Application.Pdf, "report.pdf");

        return NoContent();
    }
}
