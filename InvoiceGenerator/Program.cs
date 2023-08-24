using CsvHelper;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using System.Globalization;
using QuestPDF.Helpers;
using System.Diagnostics;

using var reader = new StreamReader("Invoice.csv");
using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
var workItems = csv.GetRecords<WorkItem>().ToArray();

var invoice = new Invoice(
    1,
    DateTime.Parse("2023/09/01"),
    DateTime.Parse("2023/09/10"),
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
QuestPDF.Settings.License = LicenseType.Community;
const string pdfFilePath = "Invoice.pdf";
var document = new InvoiceDocument(invoice);
document.GeneratePdf(pdfFilePath);
Process.Start("explorer.exe", pdfFilePath);

file class InvoiceDocument : IDocument
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
                    text.Span($"{Model.IssueDate:d}");
                });

                column.Item().Text(text =>
                {
                    text.Span("Due date: ").SemiBold();
                    text.Span($"{Model.DueDate:d}");
                });
            });

            var topoLogoImage = Image.FromFile("Topo-Logo.png");
            row.ConstantItem(100).Height(50).Image(topoLogoImage);
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


file class AddressComponent : IComponent
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


file record Address(string CompanyName, string Street, string City, string State, string Email, string Phone);

file record Invoice(int Number, DateTime IssueDate, DateTime DueDate, Address From, Address For, WorkItem[] WorkItems);

file record WorkItem(DateTime Date, int Hours, string Work);