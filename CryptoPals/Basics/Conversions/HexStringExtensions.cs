using System;
using System.Text;

namespace CryptoPals.Basics.Conversions;

public static class HexStringExtensions
{
    public static byte[] FromHex(this string hexString)
    {
        if (hexString.Length % 2 != 0)
            throw new ArgumentException("Hex string cannot have an odd number of characters.", nameof(hexString));

        var bytes = new byte[hexString.Length / 2];

        checked
        {
            for (int i = 0; i < bytes.Length; i++)
                bytes[i] = (byte) (HexDigitToNibble(hexString[2 * i]) * 16 + HexDigitToNibble(hexString[2 * i + 1]));
        }

        return bytes;
    }

    private static byte HexDigitToNibble(char hexDigit) => hexDigit switch
    {
        >= '0' and <= '9' => (byte)(hexDigit - '0'),
        >= 'A' and <= 'F' => (byte)(hexDigit - 'A' + 10),
        >= 'a' and <= 'f' => (byte)(hexDigit - 'a' + 10),
        _ => throw new ArgumentOutOfRangeException(nameof(hexDigit))
    };

    public static string ToHex(this byte[] bytes)
    {
        var stringBuilder = new StringBuilder(2 * bytes.Length);

        foreach (var @byte in bytes)
        {
            stringBuilder.Append(NibbleToHexDigit(@byte >> 4));
            stringBuilder.Append(NibbleToHexDigit(@byte & 0xF));
        }

        return stringBuilder.ToString();
    }

    private static char NibbleToHexDigit(int nibble) => nibble switch
    {
        >=  0 and < 10 => (char)('0' + nibble),
        >= 10 and < 16 => (char)('a' + nibble - 10),
        _ => throw new ArgumentOutOfRangeException(nameof(nibble))
    };
}