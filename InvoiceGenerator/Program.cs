using CsvHelper;
using QuestPDF;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using System.Globalization;
using QuestPDF.Helpers;
using System.Diagnostics;

using var reader = new StreamReader("Invoice.csv");
using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
var workItems = csv.GetRecords<WorkItem>().ToArray();
var dateTimeToday = DateTime.Today;
var today = DateOnly.FromDateTime(dateTimeToday);
#pragma warning disable S6580 // Use a format provider when parsing date and time
var invoiceNumber = (int)((DateTime.Today - DateTime.Parse("2023/08/01")).TotalDays / 30);
#pragma warning restore S6580 // Use a format provider when parsing date and time
var invoice = new Invoice(
    invoiceNumber,
    DateOnly.Parse($"{today.Year}/{today.Month}/01", CultureInfo.InvariantCulture),
    DateOnly.Parse($"{today.Year}/{today.Month}/10", CultureInfo.InvariantCulture),
    new Address("Hailin Shu",
        "JinShanSi Village, GuangPing town, NingQiang county",
        "HanZhong city",
        "ShanXi province, China",
        "imgen@hotmail.com",
        "+86-18823214892"),
    new Address("Topo Solutions Limited",
        "Unit 11-16, 28/F, Two Sky Parc",
        "51 Hung To Road, Kwun Tong",
        "Hong Kong",
        "hello@topo.cc",
        "+852 3018 8089"),
    workItems
);
Settings.License = LicenseType.Community;
var yearMonth = dateTimeToday.AddDays(-20).ToString("Y", CultureInfo.InvariantCulture);
var fileName = $"Invoice of {yearMonth}.pdf";
var pdfFilePath = Path.Combine(Environment.CurrentDirectory, fileName);
var document = new InvoiceDocument(invoice);
document.GeneratePdf(pdfFilePath);
var process = new Process { StartInfo = new ProcessStartInfo(pdfFilePath) { UseShellExecute = true } }; 
process.Start();
await process.WaitForExitAsync();
var desktopDir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
var destinationPath = Path.Combine(desktopDir, fileName);
File.Copy(pdfFilePath, destinationPath, overwrite: true);

file sealed class InvoiceDocument : IDocument
{
    private Invoice Model { get; }

    public InvoiceDocument(Invoice model) => Model = model;

    public DocumentMetadata GetMetadata() => DocumentMetadata.Default;
    public DocumentSettings GetSettings() => DocumentSettings.Default;

    public void Compose(IDocumentContainer container)
    {
        container
            .Page(page =>
            {
                page.Margin(50);

                page.Header().Element(ComposeHeader);
                page.Content().Element(ComposeContent);


                page.Footer().AlignCenter().Text(x =>
                {
                    x.CurrentPageNumber();
                    x.Span(" / ");
                    x.TotalPages();
                });
            });
    }

    private void ComposeHeader(IContainer container)
    {
        var titleStyle = TextStyle.Default.FontSize(20).SemiBold().FontColor(Colors.Blue.Medium);

        container.Row(row =>
        {
            row.RelativeItem().Column(column =>
            {
                column.Item().Text($"Invoice #{Model.Number}").Style(titleStyle);

                column.Item().Text(text =>
                {
                    text.Span("Issue date: ").SemiBold();
                    text.Span($"{Model.IssueDate:yyyy/MM/dd}");
                });

                column.Item().Text(text =>
                {
                    text.Span("Due date: ").SemiBold();
                    text.Span($"{Model.DueDate:yyyy/MM/dd}");
                });
            });
        });
    }

    private void ComposeContent(IContainer container)
    {
        container.PaddingVertical(40).Column(column =>
        {
            column.Spacing(5);

            column.Item().Row(row =>
            {
                row.RelativeItem().Component(new AddressComponent("From", Model.From));
                row.ConstantItem(50);
                row.RelativeItem().Component(new AddressComponent("For", Model.For));
            });

            column.Item().Element(ComposeTable);

            var totalHours = Model.WorkItems.Sum(x => x.Hours);
            const int hourlyRate = 400;
            var totalAmountToBePaid = totalHours * hourlyRate;
            column.Item().AlignRight().Text($"Total hours: {totalHours}. Total amount to be paid: {totalAmountToBePaid} HKD").FontSize(14);
        });
    }

    private void ComposeTable(IContainer container)
    {
        container.Table(table =>
        {
            // step 1
            table.ColumnsDefinition(columns =>
            {
                columns.ConstantColumn(70);
                columns.RelativeColumn(4);
                columns.RelativeColumn();
            });

            // step 2
            table.Header(header =>
            {
                header.Cell().Element(CellStyle).Text("Date");
                header.Cell().Element(CellStyle).Text("Work");
                header.Cell().Element(CellStyle).AlignRight().Text("Hours");

                static IContainer CellStyle(IContainer container)
                {
                    return container.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black);
                }
            });

            // step 3
            foreach (var item in Model.WorkItems)
            {
                table.Cell().Element(CellStyle).Text(item.Date.ToShortDateString());
                table.Cell().Element(CellStyle).Text(item.Work);
                table.Cell().Element(CellStyle).AlignRight().Text(item.Hours.ToString());

                static IContainer CellStyle(IContainer container)
                {
                    return container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5);
                }
            }
        });
    }
}


file sealed class AddressComponent : IComponent
{
    private string Title { get; }
    private Address Address { get; }

    public AddressComponent(string title, Address address)
    {
        Title = title;
        Address = address;
    }

    public void Compose(IContainer container)
    {
        container.Column(column =>
        {
            column.Spacing(2);

            column.Item().BorderBottom(1).PaddingBottom(5).Text(Title).SemiBold();

            column.Item().Text(Address.CompanyName);
            column.Item().Text(Address.Street);
            column.Item().Text($"{Address.City}, {Address.State}");
            column.Item().Text(Address.Email);
            column.Item().Text(Address.Phone);
        });
    }
}


file sealed record Address(string CompanyName, string Street, string City, string State, string Email, string Phone);

file sealed record Invoice(int Number, DateOnly IssueDate, DateOnly DueDate, Address From, Address For, WorkItem[] WorkItems);

// ReSharper disable once ClassNeverInstantiated.Local
file sealed record WorkItem(DateTime Date, int Hours, string Work);