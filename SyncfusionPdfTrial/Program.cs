// See https://aka.ms/new-console-template for more information

using Syncfusion.Licensing;
using Syncfusion.Pdf.Parsing;
using Syncfusion.Pdf.Security;

SyncfusionLicenseProvider.RegisterLicense("NRAiBiAaIQQuGjN/V0R+Xk9NfV5AQmBIYVp/TGpJfl96cVxMZVVBJAtUQF1hSn5bd0BjWHtZcHRRRmFV");
var fileStream = new FileStream("Invoice.pdf", FileMode.Open, FileAccess.Read);
var pdfDoc = new PdfLoadedDocument(fileStream);
var pfxFileStream = new FileStream("AATL2.pfx", FileMode.Open);
var pdfCertificate = new PdfCertificate(pfxFileStream, "ja3Aq*sf9XnGyrseoDtRk");
var signature = new PdfSignature(pdfDoc, pdfDoc.Pages[0], pdfCertificate, "Topo Solutions Limited");
pdfDoc.Save(new FileStream(
    Path.Combine(Environment.CurrentDirectory, "Invoice_signed.pdf"),
    FileMode.Create
));

