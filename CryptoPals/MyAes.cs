using System;
using System.IO;
using System.Security.Cryptography;
using CryptoPals.Basics.XOR;

namespace CryptoPals;

public static class MyAes
{
    #region ECB (Electronic Code Book) mode

    // https://cryptopals.com/sets/1/challenges/7
    // this one time, I am allowed to cheat, so I will.

    public static byte[] EncryptEcb(byte[] plaintext, byte[] key)
    {
        using var aes = Aes.Create();
        aes.Mode = CipherMode.ECB;
        aes.Padding = PaddingMode.None;
        aes.KeySize = 8 * key.Length;
        aes.Key = key;

        using var outputStream = new MemoryStream();
        using var encryptor = aes.CreateEncryptor();
        using var cryptoStream = new CryptoStream(outputStream, encryptor, CryptoStreamMode.Write);

        cryptoStream.Write(plaintext);
        return outputStream.ToArray();
    }

    public static byte[] DecryptEcb(byte[] ciphertext, byte[] key)
    {
        using var aes = Aes.Create();
        aes.Mode = CipherMode.ECB;
        aes.Padding = PaddingMode.None;
        aes.KeySize = 8 * key.Length;
        aes.Key = key;

        using var outputStream = new MemoryStream();
        using var decryptor = aes.CreateDecryptor();
        using var cryptoStream = new CryptoStream(outputStream, decryptor, CryptoStreamMode.Write);

        cryptoStream.Write(ciphertext);
        return outputStream.ToArray();
    }

    #endregion

    #region CBC (Cipher Block Chaining)

    // https://cryptopals.com/sets/2/challenges/10

    public static byte[] EncryptCbc(byte[] plaintext, byte[] key, byte[] initialisationVector)
    {
        if (key.Length != initialisationVector.Length)
            throw new ArgumentException("The key and initialisation vectors must have the same length");

        int blockSize = key.Length;
        if (plaintext.Length % blockSize != 0)
            throw new ArgumentException($"The plaintext length must be a multiple of the key size ({blockSize * 8} bits)");

        var result = new byte[plaintext.Length];
        var offset = 0;
        var runningState = new byte[blockSize];
        initialisationVector.CopyTo(runningState.AsSpan());

        while (offset < plaintext.Length)
        {
            byte[] block = plaintext[offset..(offset + blockSize)];

            byte[] encryptedBlock = EncryptEcb(FixedXOR.Combine(block, runningState), key);
            encryptedBlock.CopyTo(result.AsSpan(offset, blockSize));

            runningState = encryptedBlock;
            offset += blockSize;
        }

        return result;
    }

    public static byte[] DecryptCbc(byte[] ciphertext, byte[] key, byte[] initialisationVector)
    {
        if (key.Length != initialisationVector.Length)
            throw new ArgumentException("The key and initialisation vectors must have the same length");

        int blockSize = key.Length;
        if (ciphertext.Length % blockSize != 0)
            throw new ArgumentException($"The plaintext length must be a multiple of the key size ({blockSize * 8} bits)");

        var result = new byte[ciphertext.Length];
        var offset = 0;
        var runningState = new byte[blockSize];
        initialisationVector.CopyTo(runningState.AsSpan());

        while (offset < ciphertext.Length)
        {
            byte[] block = ciphertext[offset..(offset + blockSize)];

            // compare encryption - function application order is inverted!
            // which, after a bit of thought, makes sense. as math says: (f ⚬ g)⁻¹ = (g⁻¹ ⚬ f⁻¹)
            // (wherein ⚬ denotes the function composition operation).
            // note that here xor is in a way also an inverse of itself, as in xor(xor(x, y), y) = x.
            byte[] decryptedBlock = FixedXOR.Combine(runningState, DecryptEcb(block, key));
            decryptedBlock.CopyTo(result.AsSpan(offset, blockSize));

            runningState = block;
            offset += blockSize;
        }

        return result;
    }

    #endregion
}