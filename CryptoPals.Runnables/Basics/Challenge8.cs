using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CryptoPals.Runnables.Basics
{
    [Runnable(8)]
    public class Challenge8 : IRunnable
    {
        public void Run(string[] args)
        {
            if (args.Length != 1)
                throw new ArgumentException("Please provide a single path to a file with hex-encoded " +
                                            "ciphertexts.");

            string filePath = args.Single();

            // I'm gonna leave these hex-encoded as an exception,
            // since comparing is going to be much easier on hex-encoded strings.
            // just need to remember to check 32 chars at a time...
            string[] hexEncodedCiphertexts = File.ReadAllText(filePath)
                .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                .ToArray();

            double maxRepeatingBlockProportion = 0;
            int mostSuspiciousCiphertextIndex = -1;

            for (int i = 0; i < hexEncodedCiphertexts.Length; ++i)
            {
                string ciphertext = hexEncodedCiphertexts[i];
                int blockCount = ciphertext.Length / 32;

                var occurrenceCounts = new Dictionary<string, int>();

                while (!string.IsNullOrEmpty(ciphertext))
                {
                    string block = ciphertext[..32];
                    ciphertext = ciphertext[32..];

                    if (!occurrenceCounts.ContainsKey(block))
                        occurrenceCounts[block] = 0;

                    occurrenceCounts[block] += 1;
                }

                var repeatingProportion = (double)occurrenceCounts.Max(kv => kv.Value) / blockCount;
                if (repeatingProportion > maxRepeatingBlockProportion)
                {
                    maxRepeatingBlockProportion = repeatingProportion;
                    mostSuspiciousCiphertextIndex = i;
                }
            }

            Console.WriteLine($"Ciphertext {mostSuspiciousCiphertextIndex} is most suspicious, " +
                              $"with {maxRepeatingBlockProportion:P0} of all 16-byte blocks repeating.");
        }
    }
}