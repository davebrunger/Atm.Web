namespace Atm.Web.Extensions;

public static class StringExtensions
{
    public static string Obfuscate(this string source)
    {
        if (string.IsNullOrEmpty(source))
        {
            return source;
        }
        var hiddenChars = source.Length - 4;
        return new string(source.Select((c, i) => i < hiddenChars ? '*' : c).ToArray());
    }
}
