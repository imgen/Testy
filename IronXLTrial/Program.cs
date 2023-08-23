// See https://aka.ms/new-console-template for more information
using IronXL;

IronXL.License.LicenseKey = "IRONSUITE.HS.TOPO.CC.7939-D602816371-A2C3AACCEBDBX7I6-EJCPHVYJRYUB-HUUWDDTJLIQP-J6NFTDAYOARM-S63MFWW22PKM-PENZXCKAQDEX-M3R4WJ-T4BRFMOBNECKUA-DEPLOYMENT.TRIAL-6RRGIE.TRIAL.EXPIRES.14.SEP.2023";

var xlsWorkbook = WorkBook.Create(ExcelFileFormat.XLSX);
xlsWorkbook.Metadata.Author = "IronXL";
var xlsSheet = xlsWorkbook.CreateWorkSheet("new_sheet");
xlsSheet["A1"].Value = "Hello World";
xlsSheet["A2"].Style.BottomBorder.SetColor("#ff6600");
xlsSheet["A2"].Style.BottomBorder.Type = IronXL.Styles.BorderType.Double;
var image = xlsSheet.InsertImage("Beauty.jpg", 1, 1, 2, 2);
xlsWorkbook.SaveAs("NewExcelFile.xlsx"); //Save the excel file
