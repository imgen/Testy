// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using System.Globalization;
using IronXL;

License.LicenseKey = "IRONSUITE.DEVTEAM.IRO231009.4993.48123-898138E282-ACRME4DB56NUSYLB-YJSNGBKKMZ4T-KY4HHUUTIWLT-3SFX7HUJHUXA-NVGCYP27FM5S-U24W2ZW235DN-BDH5YS-L7EXDTQRTASUUA-IRONSUITE.PROFESSIONAL.SAAS.OEM.5YR-7MWTO7.RENEW.SUPPORT.07.OCT.2028";

var xlsWorkbook = WorkBook.Create(ExcelFileFormat.XLSX);
xlsWorkbook.Metadata.Author = "IronXL";
var xlsSheet = xlsWorkbook.CreateWorkSheet("new_sheet");
if (DateOnly.TryParse("2022/02/02", CultureInfo.InvariantCulture,  out var date))
{
    xlsSheet["A1"].Value = date.ToDateTime(TimeOnly.MinValue);
    xlsSheet["A1"].FormatString = "yyyy-MM-dd";
}

var excelFilePath = Path.Combine(Environment.CurrentDirectory, "NewExcelFile.xlsx");
xlsWorkbook.SaveAs(excelFilePath); //Save the excel file
Process.Start("explorer.exe", excelFilePath);
