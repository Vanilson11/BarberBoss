
using BarberBoss.Application.UseCases.Revenues.Reports.Pdf.Fonts;
using BarberBoss.Domain;
using BarberBoss.Domain.Entities;
using BarberBoss.Domain.Enums;
using BarberBoss.Domain.Repositories;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using PdfSharp.Fonts;
using System.Reflection;

namespace BarberBoss.Application.UseCases.Revenues.Reports.Pdf;
public class ReportRevenuePdfUseCase : IReportRevenuePdfUseCase
{
    private const string CURRENT_SYMBOL = "R$";
    private const int HEIGHT_ROW_TABLE = 20;
    private readonly IReportsReadOnlyRepository _repository;
    public ReportRevenuePdfUseCase(IReportsReadOnlyRepository repository)
    {
        _repository = repository;
        GlobalFontSettings.FontResolver = new RevenuesFontResolver();
    }
    public async Task<byte[]> Execute(DateOnly startWeek, DateOnly endWeek)
    {
        var revenues = await _repository.GetByWeek(startWeek, endWeek);

        if (revenues.Count == 0)
            return [];

        var document = CreateDocument();

        var section = AddAndConfigSection(document);

        CreateTableHeader(section);

        var paragraph = section.AddParagraph();
        InsertTitleOnPage(paragraph, revenues);

        var table = CreateTableForRevenues(section);
        
        foreach (var revenue in revenues)
        {
            var row = table.AddRow();
            row.Height = HEIGHT_ROW_TABLE;

            AddRevenueTitle(row.Cells[0], revenue.Title);

            AddHeaderForAmount(row.Cells[3]);

            row = table.AddRow();
            row.Height = HEIGHT_ROW_TABLE;
            row.Cells[0].AddParagraph(revenue.Date.ToString("D"));
            SetStyleForRowRevenueInformation(row.Cells[0]);

            row.Cells[1].AddParagraph(revenue.Date.ToString("t"));
            SetStyleForRowRevenueInformation(row.Cells[1]);

            row.Cells[2].AddParagraph(ConvertPaymentType(revenue.PaymentType));
            SetStyleForRowRevenueInformation(row.Cells[2]);

            row.Cells[3].AddParagraph($"{CURRENT_SYMBOL} {revenue.Amount}");
            row.Cells[3].Format.Font = new Font { Name = FontsHelper.ROBOTO_REGULAR, Size = 10, Color = ColorsHelper.BLACK };
            row.Cells[3].Shading.Color = ColorsHelper.WHITE;
            row.Cells[3].VerticalAlignment = VerticalAlignment.Center;
            row.Cells[3].MergeDown = 1;

            if(string.IsNullOrWhiteSpace(revenue.Description) == false)
            {
                var rowDescription = table.AddRow();
                rowDescription.Height = HEIGHT_ROW_TABLE;
                rowDescription.Cells[0].MergeRight = 2;

                rowDescription.Cells[0].AddParagraph(revenue.Description);
                rowDescription.Cells[0].Format.Font = new Font { Name = FontsHelper.ROBOTO_REGULAR, Size = 9, Color = ColorsHelper.GRAY_DARK };
                rowDescription.Cells[0].Shading.Color = ColorsHelper.GRAY_LIGHT;
                rowDescription.Cells[0].VerticalAlignment = VerticalAlignment.Center;
                rowDescription.Cells[0].Format.LeftIndent = 20;
            }
        }

        return RenderDocument(document);
    }

    private Document CreateDocument()
    {
        var document = new Document();
        document.Info.Author = "Vanilson Sousa";
        document.Info.Title = ResourceReportsMessages.REPORT_REVENUES_OFF_WEEK;

        var style = document.Styles["Normal"];
        style!.Font.Name = FontsHelper.DEFAULT;

        return document;
    }

    private Section AddAndConfigSection(Document document)
    {
        var page = document.AddSection();

        page.PageSetup = document.DefaultPageSetup.Clone();
        page.PageSetup.PageFormat = PageFormat.A4;

        page.PageSetup.TopMargin = 53;
        page.PageSetup.RightMargin = 35;
        page.PageSetup.LeftMargin = 35;
        page.PageSetup.BottomMargin = 53;

        return page;
    }

    private void CreateTableHeader(Section section)
    {
        var table = section.AddTable();
        InsertHeaderWithLogoAndText(table);
    }

    private void InsertHeaderWithLogoAndText(Table table)
    {
        table.AddColumn();
        table.AddColumn("300");

        var row = table.AddRow();

        var assembly = Assembly.GetExecutingAssembly();
        var directoryName = Path.GetDirectoryName(assembly.Location);
        var pathFile = Path.Combine(directoryName!, "Logo", "barber-logo2.png");

        row.Cells[0].AddImage(pathFile);
        row.Cells[1].AddParagraph(ResourceReportsMessages.BARBER_NAME);
        row.Cells[1].Format.LeftIndent = 20;
        row.Cells[1].Format.Font = new Font { Name = FontsHelper.BEBAS_NEUE_REGULAR, Size = 25 };
        row.Cells[1].VerticalAlignment = MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Center;
    }

    private void InsertTitleOnPage(Paragraph paragraph, List<Revenue> revenues)
    {
        paragraph.Format.SpaceBefore = "38";

        paragraph.AddFormattedText(ResourceReportsMessages.REVENUE_OFF_WEEK, new Font { Name = FontsHelper.ROBOTO_MEDIUM, Size = 15 });
        paragraph.Format.Alignment = ParagraphAlignment.Left;
        paragraph.AddLineBreak();

        var totAmountRevenues = revenues.Sum(revenue => revenue.Amount);

        paragraph.AddFormattedText($"{CURRENT_SYMBOL} {totAmountRevenues}", new Font { Name = FontsHelper.BEBAS_NEUE_REGULAR, Size = 50 });
        paragraph.Format.SpaceAfter = "64";
    }

    private Table CreateTableForRevenues(Section section)
    {
        var table = section.AddTable();

        table.AddColumn("195").Format.Alignment = ParagraphAlignment.Left;
        table.AddColumn("80").Format.Alignment = ParagraphAlignment.Center;
        table.AddColumn("120").Format.Alignment = ParagraphAlignment.Center;
        table.AddColumn("120").Format.Alignment = ParagraphAlignment.Right;

        return table;
    }

    private void AddRevenueTitle(Cell cell, string title)
    {
        cell.AddParagraph(title);
        cell.Format.Font = new Font { Name = FontsHelper.BEBAS_NEUE_REGULAR, Size = 15, Color = ColorsHelper.WHITE };
        cell.Shading.Color = ColorsHelper.GREEN_DARK;
        cell.VerticalAlignment = VerticalAlignment.Center;
        cell.MergeRight = 2;
        cell.Format.LeftIndent = 20;
    }

    private void AddHeaderForAmount(Cell cell)
    {
        cell.AddParagraph(ResourceReportsMessages.AMOUNT);
        cell.Format.Font = new Font { Name = FontsHelper.BEBAS_NEUE_REGULAR, Size = 14, Color = ColorsHelper.WHITE };
        cell.Shading.Color = ColorsHelper.GREEN_LIGHT;
        cell.VerticalAlignment = VerticalAlignment.Center;
    }

    private void SetStyleForRowRevenueInformation(Cell cell)
    {
        cell.Format.Font = new Font { Name = FontsHelper.ROBOTO_REGULAR, Size = 10, Color = ColorsHelper.BLACK };
        cell.Format.LeftIndent = 20;
        cell.Shading.Color = ColorsHelper.GRAY;
        cell.VerticalAlignment = VerticalAlignment.Center;
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
    private byte[] RenderDocument(Document document)
    {
        var renderer = new PdfDocumentRenderer { Document = document };

        renderer.RenderDocument();

        using var file = new MemoryStream();
        renderer.PdfDocument.Save(file);

        return file.ToArray();
    }
}
