using System;
using System.IO;
using System.Linq;
using CryptoPals.Basics.Conversions;
using CryptoPals.Basics.XOR;
using JetBrains.Annotations;

namespace CryptoPals.Runnables.Basics
{
    // https://cryptopals.com/sets/1/challenges/4
    [Runnable(4)]
    [UsedImplicitly]
    public class Challenge4 : IRunnable
    {
        public void Run(string[] args)
        {
            if (args.Length != 1)
                throw new ArgumentException("Please provide a single path to a file with newline-separated hex" +
                                            "strings to guess the encoding of.");

            string filePath = args.Single();
            string[] hexStrings = File.ReadAllText(filePath)
                .Split(new[] {'\r', '\n'}, StringSplitOptions.RemoveEmptyEntries)
                .ToArray();

            var detector = new SingleByteXORCipherDetector();
            var guessesForEachString = hexStrings.ToDictionary(
                cipherText => cipherText,
                cipherText =>
                {
                    byte[] cipher = cipherText.FromHex();
                    return detector.TryFindPlaintext(cipher);
                });

            Console.WriteLine("Best guesses for single-byte XOR key for each string:");
            Console.WriteLine();

            var header = $"{"Ciphertext".PadRight(60)} | Key | {"Plaintext".PadRight(40)} | {"Score".PadLeft(10)}";
            Console.WriteLine(header);
            Console.WriteLine(new string(Enumerable.Repeat('-', header.Length).ToArray()));

            var sortedByBestGuess = guessesForEachString
                .OrderByDescending(guesses => guesses.Value.First().Score);

            foreach (var guessForString in sortedByBestGuess)
            {
                string printablePlaintext = new string(guessForString.Value.First().Plaintext
                    .Where(c => c >= 32)
                    .ToArray());
                Console.WriteLine(
                    $"{guessForString.Key.PadRight(60)} | {guessForString.Value.First().Key.ToString().PadLeft(3)} | {printablePlaintext.PadRight(40)} | {guessForString.Value.First().Score,10:0.0000}");
            }
        }
    }
}
