// See https://aka.ms/new-console-template for more 
using IronPdf.Signing;

License.LicenseKey = "IRONSUITE.DEVTEAM.IRO231009.4993.48123-898138E282-ACRME4DB56NUSYLB-YJSNGBKKMZ4T-KY4HHUUTIWLT-3SFX7HUJHUXA-NVGCYP27FM5S-U24W2ZW235DN-BDH5YS-L7EXDTQRTASUUA-IRONSUITE.PROFESSIONAL.SAAS.OEM.5YR-7MWTO7.RENEW.SUPPORT.07.OCT.2028";
var signature = new PdfSignature("AATL2.pfx", "ja3Aq*sf9XnGyrseoDtRk");

var pdf = PdfDocument.FromFile("Invoice.pdf");
pdf.Sign(signature);
pdf.SaveAs("Invoice_signed.pdf");