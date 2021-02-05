using System.Collections.Generic;
using System.Linq;

namespace CryptoPals.Basics
{
    public class SingleByteXORCipherDetector
    {
        public ICollection<Guess> TryFindPlaintext(byte[] ciphertext)
        {
            // skip the non-printables, try everything else.
            IEnumerable<byte> candidateKeys = Enumerable.Range(0, 256)
                .Skip(32)
                .Select(i => (byte)i);

            var guesses = new List<Guess>();

            foreach (byte key in candidateKeys)
            {
                byte[] repeatedKey = Enumerable.Repeat(key, ciphertext.Length).ToArray();
                byte[] plaintextGuess = FixedXOR.Combine(ciphertext, repeatedKey);
                double score = plaintextGuess.Aggregate<byte, double>(0,
                    (subtotal, c) =>
                        subtotal + EnglishRelativeCharacterFrequency.GetValueOrDefault(char.ToLower((char) c)));

                guesses.Add(
                    new Guess((char) key,
                    new string(plaintextGuess.Select(b => (char)b).ToArray()),
                    score / ciphertext.Length));
            }

            return guesses.OrderByDescending(g => g.Score).ToList();
        }

        public record Guess
        {
            public char Key { get; }
            public string Plaintext { get; }
            public double Score { get; }

            public Guess(char key, string plaintext, double score)
                => (Key, Plaintext, Score) = (key, plaintext, score);
        }

        /// <summary>
        /// As given by http://pi.math.cornell.edu/~mec/2003-2004/cryptography/subs/frequencies.html.
        /// I get the joke now!
        /// This probably shouldn't be embedded in source but this way is simple and doing it the proper way
        /// is not the point of the exercise.
        /// </summary>
        private static readonly Dictionary<char, double> EnglishRelativeCharacterFrequency = new()
        {
            ['e'] = 12.02,
            ['t'] =  9.10,
            ['a'] =  8.12,
            ['o'] =  7.68,
            ['i'] =  7.31,
            ['n'] =  6.95,
            ['s'] =  6.28,
            ['r'] =  6.02,
            ['h'] =  5.92,
            ['d'] =  4.32,
            ['l'] =  3.98,
            ['u'] =  2.88,
            ['c'] =  2.71,
            ['m'] =  2.61,
            ['f'] =  2.30,
            ['y'] =  2.11,
            ['w'] =  2.09,
            ['g'] =  2.03,
            ['p'] =  1.82,
            ['b'] =  1.49,
            ['v'] =  1.11,
            ['k'] =  0.69,
            ['x'] =  0.17,
            ['q'] =  0.11,
            ['j'] =  0.10,
            ['z'] =  0.07
        };
    }
}
