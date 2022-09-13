using System;
using System.Linq;

namespace CryptoPals.BlockCrypto;

public static class Pkcs7Padding
{
    public static byte[] Pad(byte[] plaintext, int blockSize)
    {
        int bytesToPad = blockSize - plaintext.Length;

        if (bytesToPad < 0)
            throw new ArgumentException($"The plaintext exceeds the provided block size of {blockSize}.");

        return plaintext.Concat(Enumerable.Repeat((byte)bytesToPad, bytesToPad)).ToArray();
    }
}