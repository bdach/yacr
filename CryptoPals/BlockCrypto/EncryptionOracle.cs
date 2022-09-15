using System;
using System.Security.Cryptography;

namespace CryptoPals.BlockCrypto;

public interface IEncryptionOracle
{
    int KeySize { get; }

    byte[] Encrypt(byte[] plaintext);
}

public class EncryptionOracle : IEncryptionOracle
{
    public int KeySize => 16;

    private readonly CipherMode _cipherMode;

    public EncryptionOracle(CipherMode cipherMode)
    {
        if (cipherMode is not (CipherMode.ECB or CipherMode.CBC))
            throw new ArgumentOutOfRangeException(nameof(cipherMode),
                "Only ECB and CBC modes are supported by this oracle.");

        _cipherMode = cipherMode;
    }

    public byte[] Encrypt(byte[] plaintext)
    {
        int prefixBytes = RandomNumberGenerator.GetInt32(5, 11);
        // for the suffix, let's just settle on symmetrically padding to a full block.
        int suffixBytes = KeySize - (prefixBytes + plaintext.Length) % KeySize;

        byte[] key = MyAes.GetRandomKey(KeySize);

        var input = new byte[prefixBytes + plaintext.Length + suffixBytes];
        RandomNumberGenerator.Fill(input.AsSpan(0, prefixBytes));
        plaintext.CopyTo(input.AsSpan(prefixBytes, plaintext.Length));
        RandomNumberGenerator.Fill(input.AsSpan(prefixBytes + plaintext.Length, suffixBytes));

        if (_cipherMode == CipherMode.ECB)
            return MyAes.EncryptEcb(input, key);

        byte[] initialisationVector = RandomNumberGenerator.GetBytes(KeySize);
        return MyAes.EncryptCbc(input, key, initialisationVector);
    }
}