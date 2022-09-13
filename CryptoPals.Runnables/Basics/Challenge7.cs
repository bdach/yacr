using System;
using System.Linq;
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

            byte[] decryptedBytes = MyAes.DecryptEcb(ciphertext, Encoding.ASCII.GetBytes("YELLOW SUBMARINE"));
            Console.WriteLine($"The plaintext is:\n{Encoding.ASCII.GetString(decryptedBytes)}");
        }
    }
}
