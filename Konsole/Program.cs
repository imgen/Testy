// See https://aka.ms/new-console-template for more information

using System.Drawing;
using System.Drawing.Imaging;

Image image;
try
{
	image = Image.FromFile(@"F:\Work\Topo\Unable-to-load.webp");
}
catch (Exception)
{
	throw;
}
if (image.RawFormat.Guid == ImageFormat.Exif.Guid)
{
    Console.WriteLine("It's an exif");
}

var ms = new MemoryStream();
image.Save(ms, ImageFormat.Exif);
Console.WriteLine("Hello, World!");
