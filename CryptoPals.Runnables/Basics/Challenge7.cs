using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace CryptoPals.Runnables.Basics
{
    [Runnable(7)]
    public class Challenge7 : IRunnable
    {
        public void Run(string[] args)
        {
            if (args.Length != 1)
                throw new ArgumentException("Please provide a single path to a base64-encoded file" +
                                            "to decode using AES-128-ECB.");

            string filePath = args.Single();
            byte[] ciphertext = Base64File.Read(filePath);
            const int keySize = 128;

            // not sure this'll be worth wrapping into a class, so let's just use what C# provides raw.
            using var aes = Aes.Create();
            aes.Mode = CipherMode.ECB;
            aes.KeySize = keySize;
            aes.Key = Encoding.ASCII.GetBytes("YELLOW SUBMARINE");

            using var memoryStream = new MemoryStream(ciphertext);
            using var decryptor = aes.CreateDecryptor();
            using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            using var streamReader = new StreamReader(cryptoStream);

            Console.WriteLine($"The plaintext is:\n{streamReader.ReadToEnd()}");
        }
    }
}
