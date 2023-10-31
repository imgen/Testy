// See https://aka.ms/new-console-template for more 

using System.Runtime.InteropServices;
using IronPdf.Signing;

License.LicenseKey = "IRONSUITE.DEVTEAM.IRO231009.4993.48123-898138E282-ACRME4DB56NUSYLB-YJSNGBKKMZ4T-KY4HHUUTIWLT-3SFX7HUJHUXA-NVGCYP27FM5S-U24W2ZW235DN-BDH5YS-L7EXDTQRTASUUA-IRONSUITE.PROFESSIONAL.SAAS.OEM.5YR-7MWTO7.RENEW.SUPPORT.07.OCT.2028";

var certFilePath = Path.Combine(Environment.CurrentDirectory, "AATL2.pfx");
var signature = new PdfSignature(certFilePath, "ja3Aq*sf9XnGyrseoDtRk")
{
    TimeStampUrl = "http://aatl-timestamp.globalsign.com/tsa/v4v5effk07zor410rew22z"
};

var pdf = PdfDocument.FromFile("Invoice.pdf");
pdf.Sign(signature);
pdf.SaveAs("Invoice_signed.pdf");