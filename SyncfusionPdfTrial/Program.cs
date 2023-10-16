// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using Syncfusion.Licensing;
using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf.Security;

SyncfusionLicenseProvider.RegisterLicense("NRAiBiAaIQQuGjN/V0R+Xk9NfV5AQmBIYVp/TGpJfl96cVxMZVVBJAtUQF1hSn5bd0BjWHtZcHRRRmFV");
var fileStream = new FileStream("Invoice.pdf", FileMode.Open, FileAccess.Read);
var pdfDoc = new PdfLoadedDocument(fileStream);
var pfxFileStream = new FileStream("AATL2.pfx", FileMode.Open);
var pdfCertificate = new PdfCertificate(pfxFileStream, "ja3Aq*sf9XnGyrseoDtRk");
#pragma warning disable S1481
var pdfSignature = new PdfSignature(pdfDoc, pdfDoc.Pages[0], pdfCertificate, "Topo Solutions Limited")
#pragma warning restore S1481
{
        TimeStampServer = new TimeStampServer(new Uri("http://aatl-timestamp.globalsign.com/tsa/v4v5effk07zor410rew22z"))
    };

var signedPdfFilePath = Path.Combine(Environment.CurrentDirectory, "Invoice_signed.pdf");
pdfDoc.Save(new FileStream(signedPdfFilePath, FileMode.Create));
Process.Start("explorer.exe", signedPdfFilePath);

