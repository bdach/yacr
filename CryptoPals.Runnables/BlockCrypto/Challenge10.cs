using System;
using System.Linq;
using System.Text;

namespace CryptoPals.Runnables.BlockCrypto;

[Runnable(10)]
public class Challenge10 : IRunnable
{
    public void Run(string[] args)
    {
        if (args.Length != 1)
            throw new ArgumentException("Please provide a single path to a base64-encoded file " +
                                        "to decode using AES-128-CBC.");

        string filePath = args.Single();
        byte[] ciphertext = Base64File.Read(filePath);

        const string key = "YELLOW SUBMARINE";

        Console.WriteLine($"Decrypting using key {key} and null-byte-stuffed IV.");

        byte[] keyBytes = Encoding.ASCII.GetBytes(key);
        var ivBytes = new byte[keyBytes.Length];

        byte[] decryptedBytes = MyAes.DecryptCbc(ciphertext, keyBytes, ivBytes);
        Console.Write($"The plaintext is:\n{Encoding.ASCII.GetString(decryptedBytes)}");
    }
}