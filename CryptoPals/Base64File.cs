using System.IO;
using CryptoPals.Basics.Conversions;

namespace CryptoPals;

public static class Base64File
{
    public static byte[] Read(string path)
        => File.ReadAllText(path)
            .Replace("\r", "")
            .Replace("\n", "")
            .FromBase64();
}