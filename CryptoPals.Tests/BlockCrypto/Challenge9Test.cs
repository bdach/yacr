using System;
using System.Text;
using CryptoPals.BlockCrypto;
using NUnit.Framework;

namespace CryptoPals.Tests.BlockCrypto;

[TestFixture]
public class Challenge9Test
{
    [TestCase("YELLOW SUBMAR", 20, "YELLOW SUBMAR\x7\x7\x7\x7\x7\x7\x7")]
    [TestCase("YELLOW SUBMARINE", 20, "YELLOW SUBMARINE\x4\x4\x4\x4")]
    [TestCase("YELLOW SUBMARINE 123", 20, "YELLOW SUBMARINE 123")]
    public void TestPkcs7Padding(string plaintext, int blockSize, string paddedPlaintext)
    {
        byte[] bytesToPad = Encoding.ASCII.GetBytes(plaintext);
        byte[] expectedPaddedBytes = Encoding.ASCII.GetBytes(paddedPlaintext);

        byte[] actualPaddedBytes = Pkcs7Padding.Pad(bytesToPad, blockSize);
        Assert.That(actualPaddedBytes, Is.EquivalentTo(expectedPaddedBytes));
    }

    [Test]
    public void TestPkcs7Padding_BlockTooShort()
    {
        Assert.Throws<ArgumentException>(
            () => Pkcs7Padding.Pad(Encoding.ASCII.GetBytes("YELLOW SUBMARINE"), 8));
    }
}