namespace BarberBoss.Application.UseCases.Revenues.Reports.Pdf;
public interface IReportRevenuePdfUseCase
{
    Task<byte[]> Execute(DateOnly startWeek, DateOnly endWeek);
}
