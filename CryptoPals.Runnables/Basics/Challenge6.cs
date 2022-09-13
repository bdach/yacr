using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CryptoPals.Basics;
using CryptoPals.Basics.XOR;

namespace CryptoPals.Runnables.Basics;

// https://cryptopals.com/sets/1/challenges/6
[Runnable(6)]
public class Challenge6 : IRunnable
{
    public void Run(string[] args)
    {
        if (args.Length != 1)
            throw new ArgumentException("Please provide a single path to a base64-encoded file" +
                                        "to guess the encoding of.");

        string filePath = args.Single();
        byte[] ciphertext = Base64File.Read(filePath);

        int[] estimatedKeySizes = GuessKeySize(ciphertext);

        foreach (var estimatedKeySize in estimatedKeySizes)
        {
            Console.WriteLine($"The estimated key size is {estimatedKeySize}");

            byte[][] transposed = TransposeBlocks(ciphertext, estimatedKeySize);
            var blockKeys = new List<byte>();

            var detector = new SingleByteXORCipherDetector();
            for (int i = 0; i < estimatedKeySize; ++i)
            {
                var guesses = detector.TryFindPlaintext(transposed[i]);
                blockKeys.Add((byte) guesses.First().Key);
            }

            byte[] keyBytes = blockKeys.ToArray();
            string key = Encoding.ASCII.GetString(keyBytes);

            byte[] plaintextBytes = RepeatingKeyXOR.Combine(ciphertext, keyBytes);
            string plaintext = Encoding.ASCII.GetString(plaintextBytes).AsPrintable();

            Console.WriteLine($"It seems likely that the key is {key}");
            Console.WriteLine("The plaintext with that key is:");
            Console.WriteLine(plaintext);
        }
    }

    private static int[] GuessKeySize(byte[] ciphertext)
    {
        var probableKeySizes = Enumerable.Range(2, 38);

        return probableKeySizes
            .Select(keySize => (keySize, EstimateKeySizeLikelihood(ciphertext, keySize)))
            .OrderBy(tuple => tuple.Item2)
            .Take(3)
            .Select(tuple => tuple.keySize)
            .ToArray();
    }

    private static double EstimateKeySizeLikelihood(byte[] ciphertext, int keySize)
    {
        byte[] GetBlock(int index) => ciphertext[(index * keySize)..((index + 1) * keySize)];

        byte[] firstBlock = GetBlock(0);
        byte[] secondBlock = GetBlock(1);
        byte[] thirdBlock = GetBlock(2);
        byte[] fourthBlock = GetBlock(3);

        double averageHammingDistance =
            (HammingDistance.Calculate(firstBlock, secondBlock)
             + HammingDistance.Calculate(secondBlock, thirdBlock)
             + HammingDistance.Calculate(thirdBlock, fourthBlock)) / 3.0;
        return averageHammingDistance / keySize;
    }

    private static byte[][] TransposeBlocks(byte[] ciphertext, int keySize)
    {
        List<byte>[] blocks = new List<byte>[keySize];
        for (int i = 0; i < keySize; ++i)
            blocks[i] = new List<byte>();

        for (int i = 0; i < ciphertext.Length; ++i)
            blocks[i % keySize].Add(ciphertext[i]);

        return blocks.Select(block => block.ToArray()).ToArray();
    }
}