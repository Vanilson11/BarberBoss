namespace BarberBoss.Application.UseCases.Revenues.Reports.Excel;
public interface IReportRevenuesExcelUseCase
{
    Task<byte[]> Execute(DateOnly start, DateOnly end);
}
