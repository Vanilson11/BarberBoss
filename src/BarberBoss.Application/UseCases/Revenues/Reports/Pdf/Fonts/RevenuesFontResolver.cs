using PdfSharp.Fonts;
using System.Reflection;

namespace BarberBoss.Application.UseCases.Revenues.Reports.Pdf.Fonts;
public class RevenuesFontResolver : IFontResolver
{
    public byte[]? GetFont(string faceName)
    {
        var stream = ReadFontFile(faceName);

        if(stream is null)
        {
            stream = ReadFontFile(FontsHelper.DEFAULT);
        }

        var length = (int)stream!.Length;
        var data = new byte[length];

        using var file = new MemoryStream();

        stream.Read(buffer: data, offset: 0, count: length);

        return data.ToArray();
    }

    public FontResolverInfo? ResolveTypeface(string familyName, bool bold, bool italic)
    {
        return new FontResolverInfo(familyName);
    }

    private Stream? ReadFontFile(string faceName)
    {
        var assembly = Assembly.GetExecutingAssembly();
        return assembly.GetManifestResourceStream($"BarberBoss.Application.UseCases.Revenues.Reports.Pdf.Fonts.{faceName}.ttf");
    }
}
