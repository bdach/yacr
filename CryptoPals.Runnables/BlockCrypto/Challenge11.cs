using System;
using System.Security.Cryptography;
using CryptoPals.BlockCrypto;

namespace CryptoPals.Runnables.BlockCrypto;

[Runnable(11)]
public class Challenge11 : IRunnable
{
    public void Run(string[] args)
    {
        const int totalGuesses = 1000;
        var correctGuesses = 0;

        for (var i = 0; i < totalGuesses; ++i)
        {
            var cipherMode = RandomNumberGenerator.GetInt32(2) == 0
                ? CipherMode.ECB
                : CipherMode.CBC;
            var oracle = new EncryptionOracle(cipherMode);
            var detector = new BlockCipherModeDetector();

            var detectedMode = detector.Detect(oracle);

            if (cipherMode == detectedMode)
                correctGuesses += 1;
        }
        
        Console.WriteLine($"Detection accuracy from oracle over {totalGuesses} attempts: {(double)correctGuesses / totalGuesses:P0}");
    }
}