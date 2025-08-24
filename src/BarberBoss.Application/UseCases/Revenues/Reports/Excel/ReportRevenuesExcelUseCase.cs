
using BarberBoss.Domain.Repositories;
using BarberBoss.Domain;
using ClosedXML.Excel;
using BarberBoss.Domain.Enums;
using BarberBoss.Domain.Entities;

namespace BarberBoss.Application.UseCases.Revenues.Reports.Excel;
public class ReportRevenuesExcelUseCase : IReportRevenuesExcelUseCase
{
    private readonly IReportsReadOnlyRepository _repository;
    private const string CURRENT_SYMBOL = "R$";

    public ReportRevenuesExcelUseCase(IReportsReadOnlyRepository repository)
    {
        _repository = repository;
    }
    public async Task<byte[]> Execute(DateOnly start, DateOnly end)
    {
        var revenues = await _repository.GetByWeek(start, end);

        if (revenues.Count == 0)
            return [];

        var workbook = CreateWorkBook();
        
        var worksheet = workbook.Worksheets.Add($"{start:yyyy-MM-dd} a {end:yyyy-MM-dd}");
        InsertHeaderWorkSheet(worksheet);

        InserValuesOnCell(revenues, worksheet);

        using var file = new MemoryStream();
        workbook.SaveAs(file);

        return file.ToArray();
    }

    private XLWorkbook CreateWorkBook()
    {
        var workbook = new XLWorkbook();
        workbook.Author = "Vanilson Sousa";
        workbook.Style.Font.FontName = "Times New Roman";
        workbook.Style.Font.FontSize = 12;

        return workbook;
    }

    private void InsertHeaderWorkSheet(IXLWorksheet worksheet) {
        worksheet.Cell("A1").Value = ResourceReportsMessages.TITLE;
        worksheet.Cell("B1").Value = ResourceReportsMessages.DATE;
        worksheet.Cell("C1").Value = ResourceReportsMessages.PAYMENT_TYPE;
        worksheet.Cell("D1").Value = ResourceReportsMessages.AMOUNT;
        worksheet.Cell("E1").Value = ResourceReportsMessages.DESCRIPTION;

        worksheet.Column(1).Width = 35;
        worksheet.Column(2).Width = 15;
        worksheet.Column(3).Width = 20;
        worksheet.Column(4).Width = 15;
        worksheet.Column(5).Width = 50;

        

        worksheet.Cells("A1:E1").Style.Font.Bold = true;
        worksheet.Cells("A1:E1").Style.Font.FontColor = XLColor.White;
        worksheet.Cells("A1:E1").Style.Fill.BackgroundColor = XLColor.FromHtml("#205858");

        worksheet.Cell("A1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        worksheet.Cell("B1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        worksheet.Cell("C1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        worksheet.Cell("D1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
        worksheet.Cell("E1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    }

    private void InserValuesOnCell(List<Revenue> revenues, IXLWorksheet worksheet)
    {
        var row = 2;
        foreach (var revenue in revenues)
        {
            worksheet.Cell($"A{row}").Value = revenue.Title;

            worksheet.Cell($"B{row}").Value = revenue.Date;
            CenterCellContent(worksheet.Cell($"B{row}"));

            worksheet.Cell($"C{row}").Value = ConvertPaymentType(revenue.PaymentType);
            CenterCellContent(worksheet.Cell($"C{row}"));

            worksheet.Cell($"D{row}").Value = revenue.Amount;
            worksheet.Cell($"D{row}").Style.NumberFormat.Format = $"{CURRENT_SYMBOL} #, ##0.00";

            worksheet.Cell($"E{row}").Value = revenue.Description;

            row++;
        }
    }

    private string ConvertPaymentType(PaymentType paymentType)
    {
        return paymentType switch
        {
            PaymentType.Cash => ResourceReportsMessages.CASH,
            PaymentType.CreditCard => ResourceReportsMessages.CREDIT_CARD,
            PaymentType.DebitCard => ResourceReportsMessages.DEBIT_CARD,
            PaymentType.Pix => ResourceReportsMessages.PIX,
            _ => string.Empty
        };
    }

    private void CenterCellContent(IXLCell cell)
    {
        cell.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    }
}
