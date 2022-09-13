using System.Linq;

namespace CryptoPals;

public static class DecodedStringExtensions
{
    public static string AsPrintable(this string s)
        => new(s.Where(c => c >= 32).ToArray());
}