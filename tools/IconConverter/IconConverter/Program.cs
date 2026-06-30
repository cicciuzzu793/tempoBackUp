using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

var sourcePath = args.Length > 0
    ? args[0]
    : Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "..", "icona.png"));

var outputPath = args.Length > 1
    ? args[1]
    : Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "..", "tempoBackUp", "Assets", "tempoBackUp.ico"));

if (!File.Exists(sourcePath))
{
    Console.Error.WriteLine($"File sorgente non trovato: {sourcePath}");
    return 1;
}

Directory.CreateDirectory(Path.GetDirectoryName(outputPath)!);

var sizes = new[] { 16, 24, 32, 48, 64, 128, 256 };
using var source = new Bitmap(sourcePath);
var pngImages = new List<byte[]>();

foreach (var size in sizes)
{
    using var resized = new Bitmap(source, new Size(size, size));
    using var stream = new MemoryStream();
    resized.Save(stream, ImageFormat.Png);
    pngImages.Add(stream.ToArray());
}

WriteIco(outputPath, pngImages);
Console.WriteLine($"Icona creata: {outputPath}");
return 0;

static void WriteIco(string path, IReadOnlyList<byte[]> pngImages)
{
    using var stream = File.Create(path);
    using var writer = new BinaryWriter(stream);

    writer.Write((ushort)0);
    writer.Write((ushort)1);
    writer.Write((ushort)pngImages.Count);

    var offset = 6 + (16 * pngImages.Count);
    foreach (var png in pngImages)
    {
        using var imageStream = new MemoryStream(png);
        using var bitmap = new Bitmap(imageStream);
        writer.Write((byte)bitmap.Width);
        writer.Write((byte)bitmap.Height);
        writer.Write((byte)0);
        writer.Write((byte)0);
        writer.Write((ushort)1);
        writer.Write((ushort)32);
        writer.Write(png.Length);
        writer.Write(offset);
        offset += png.Length;
    }

    foreach (var png in pngImages)
    {
        writer.Write(png);
    }
}
