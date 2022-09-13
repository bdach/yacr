using System.IO;
using System.Security.Cryptography;

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
}