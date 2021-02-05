using System;
using System.Linq;
using CryptoPals.Basics;
using JetBrains.Annotations;

namespace CryptoPals.Runnables.Basics
{
    // https://cryptopals.com/sets/1/challenges/3
    [Runnable(3)]
    [UsedImplicitly]
    public class Challenge3 : IRunnable
    {
        public void Run(string[] args)
        {
            if (args.Length != 1)
                throw new ArgumentException("Please provide a single hex-encoded string to guess the encoding of.");

            string ciphertext = args.Single();
            byte[] cipher = ciphertext.FromHex();

            var detector = new SingleByteXORCipherDetector();
            var guesses = detector.TryFindPlaintext(cipher);

            Console.WriteLine($"Hex-encoded ciphertext: {ciphertext}");
            Console.WriteLine("Best guesses for single-byte XOR cipher key:");
            Console.WriteLine();

            var header = $"Key | {"Plaintext".PadRight(40)} | {"Score".PadLeft(10)}";
            Console.WriteLine(header);
            Console.WriteLine(new string(Enumerable.Repeat('-', header.Length).ToArray()));

            foreach (var guess in guesses)
            {
                string printablePlaintext = new string(guess.Plaintext
                    .Where(c => c >= 32)
                    .ToArray());
                Console.WriteLine(
                    $"{guess.Key.ToString().PadLeft(3)} | {printablePlaintext.PadRight(40)} | {guess.Score,10:0.0000}");
            }
        }
    }
}
