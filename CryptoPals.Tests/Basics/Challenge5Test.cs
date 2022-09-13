using System.Text;
using CryptoPals.Basics.Conversions;
using CryptoPals.Basics.XOR;
using NUnit.Framework;

namespace CryptoPals.Tests.Basics;

// https://cryptopals.com/sets/1/challenges/5
[TestFixture]
public class Challenge5Test
{
    [TestCase(
        "Burning 'em, if you ain't quick and nimble\nI go crazy when I hear a cymbal",
        "ICE",
        "0b3637272a2b2e63622c2e69692a23693a2a3c6324202d623d63343c2a26226324272765272a282b2f20430a652e2c652a3124333a653e2b2027630c692b20283165286326302e27282f")]
    public void TestRepeatingKeyXOR(string plaintext, string key, string expectedCiphertext)
    {
        byte[] plaintextBytes = Encoding.ASCII.GetBytes(plaintext);
        byte[] keyBytes = Encoding.ASCII.GetBytes(key);
        Assert.That(RepeatingKeyXOR.Combine(plaintextBytes, keyBytes).ToHex(), Is.EqualTo(expectedCiphertext));
    }
}