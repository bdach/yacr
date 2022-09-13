using System;
using System.Numerics;

namespace CryptoPals.Basics;

public static class HammingDistance
{
    public static int Calculate(byte[] first, byte[] second)
    {
        if (first.Length != second.Length)
            throw new InvalidOperationException(
                "Cannot calculate Hamming distance for two strings of different length");

        int distance = 0;

        checked
        {
            for (int i = 0; i < first.Length; ++i)
                distance += BitOperations.PopCount((uint) (first[i] ^ second[i]));
        }

        return distance;
    }
}