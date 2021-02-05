using System;
using System.Collections.Generic;
using System.Text;

namespace CryptoPals.Basics.Conversions
{
    public static class Base64Extensions
    {
        public static byte[] FromBase64(this string base64String)
        {
            var bytes = new List<byte>(base64String.Length / 4 * 3);

            for (int i = 0; i < base64String.Length; i += 4)
            {
                int? firstSixBits  = CharToSixBits(base64String[i]);
                int? secondSixBits = CharToSixBits(base64String[i + 1]);
                int? thirdSixBits  = CharToSixBits(base64String[i + 2]);
                int? fourthSixBits = CharToSixBits(base64String[i + 3]);

                if (firstSixBits == null || secondSixBits == null)
                    throw new ArgumentException(
                        "The first two bits taken from a four-character base64 cluster should never be null.");

                checked
                {
                    byte firstByte = (byte) ((firstSixBits.Value << 2) + ((secondSixBits.Value & 0b110000) >> 4));
                    bytes.Add(firstByte);

                    if (thirdSixBits == null)
                        break;

                    byte secondByte = (byte) (((secondSixBits.Value & 0b001111) << 4) +
                                               ((thirdSixBits.Value & 0b111100) >> 2));
                    bytes.Add(secondByte);

                    if (fourthSixBits == null)
                        break;

                    byte thirdByte = (byte) (((thirdSixBits.Value & 0b000011) << 6) + fourthSixBits.Value);
                    bytes.Add(thirdByte);
                }
            }

            return bytes.ToArray();
        }

        private static int? CharToSixBits(char base64Char) => base64Char switch
        {
            >= 'A' and <= 'Z' => base64Char - 'A',
            >= 'a' and <= 'z' => base64Char - 'a' + 26,
            >= '0' and <= '9' => base64Char - '0' + 52,
                          '+' => 62,
                          '/' => 63,
                          '=' => null,
            _ => throw new ArgumentOutOfRangeException(nameof(base64Char))
        };

        public static string ToBase64(this byte[] bytes)
        {
            var stringBuilder = new StringBuilder((int)Math.Ceiling(bytes.Length / 3.0) * 4);

            for (int i = 0; i < bytes.Length; i += 3)
            {
                int? thirdSixBits = null, fourthSixBits = null;

                int firstSixBits  = (bytes[i] & 0b1111_1100) >> 2;
                int secondSixBits = (bytes[i] & 0b0000_0011) << 4;

                if (i + 1 < bytes.Length)
                {
                    secondSixBits += (bytes[i + 1] & 0b1111_0000) >> 4;
                    thirdSixBits   = (bytes[i + 1] & 0b0000_1111) << 2;
                }

                if (i + 2 < bytes.Length)
                {
                    thirdSixBits += (bytes[i + 2] & 0b1100_0000) >> 6;
                    fourthSixBits =  bytes[i + 2] & 0b0011_1111;
                }

                stringBuilder.Append(SixBitsToChar(firstSixBits));
                stringBuilder.Append(SixBitsToChar(secondSixBits));
                stringBuilder.Append(SixBitsToChar(thirdSixBits));
                stringBuilder.Append(SixBitsToChar(fourthSixBits));
            }

            return stringBuilder.ToString();
        }

        private static char SixBitsToChar(int? sixBits) => sixBits switch
        {
            >=  0 and < 26 => (char)('A' + sixBits),
            >= 26 and < 52 => (char)('a' + sixBits - 26),
            >= 52 and < 62 => (char)('0' + sixBits - 52),
                        62 => '+',
                        63 => '/',
                      null => '=',
            _ => throw new ArgumentOutOfRangeException(nameof(sixBits))
        };
    }
}