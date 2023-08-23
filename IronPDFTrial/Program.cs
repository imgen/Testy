// See https://aka.ms/new-console-template for more 
using IronPdf.Signing;

License.LicenseKey = "IRONSUITE.HS.TOPO.CC.7939-D602816371-A2C3AACCEBDBX7I6-EJCPHVYJRYUB-HUUWDDTJLIQP-J6NFTDAYOARM-S63MFWW22PKM-PENZXCKAQDEX-M3R4WJ-T4BRFMOBNECKUA-DEPLOYMENT.TRIAL-6RRGIE.TRIAL.EXPIRES.14.SEP.2023";
var signature = new PdfSignature("AATL2.pfx", "ja3Aq*sf9XnGyrseoDtRk");

var pdf = PdfDocument.FromFile("Invoice.pdf");
pdf.Sign(signature);
pdf.SaveAs("Invoice_signed.pdf");