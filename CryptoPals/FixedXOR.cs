using System;

namespace CryptoPals
{
    public static class FixedXOR
    {
        public static byte[] Combine(byte[] first, byte[] second)
        {
            if (first.Length != second.Length)
                throw new ArgumentException($"{nameof(first)} and {nameof(second)} should have equal lengths.");

            byte[] result = new byte[first.Length];

            checked
            {
                for (int i = 0; i < first.Length; ++i)
                    result[i] = (byte) (first[i] ^ second[i]);
            }

            return result;
        }
    }
}
