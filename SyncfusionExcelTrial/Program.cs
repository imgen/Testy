// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using Syncfusion.Drawing;
using Syncfusion.Licensing;
using Syncfusion.XlsIO;

SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NHaF5cWWNCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdgWH9fdXRTRmZYUUN3VkY=");
using var excelEngine = new ExcelEngine();
var app = excelEngine.Excel;
app.DefaultVersion = ExcelVersion.Xlsx;
var workbook = app.Workbooks.Create(1);
var worksheet = workbook.Worksheets[0];
worksheet.Name = "Synced";
worksheet.Range["A1"].Value = "2022/02/02";
var cells = worksheet.Cells;
foreach (var cell in cells)
{
    cell.CellStyle.Color = Color.Azure;
    var comment = cell.AddComment();
    comment.Text = "No comment";
}

var imageFilePath = Path.Combine(Environment.CurrentDirectory, "Beauty.jpg");

var picture = new Image(new FileStream(imageFilePath, FileMode.Open));
worksheet.Pictures.AddPicture(4, 5, picture);
var xlsxFilePath = Path.Combine(Environment.CurrentDirectory, "HelloSync.xlsx");
using var stream = new FileStream(xlsxFilePath, FileMode.Create, FileAccess.ReadWrite);
workbook.SaveAs(stream);

Process.Start("explorer.exe", xlsxFilePath);

