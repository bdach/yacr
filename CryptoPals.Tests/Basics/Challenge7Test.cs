using System.Text;
using NUnit.Framework;

namespace CryptoPals.Tests.Basics;

[TestFixture]
public class Challenge7Test
{
    [Test]
    public void TestAesEcbRoundTrip()
    {
        byte[] key = Encoding.ASCII.GetBytes("YELLOW SUBMARINE");

        const string plaintext = "This is 32 bytes of text, really";
        byte[] plaintextBytes = Encoding.ASCII.GetBytes(plaintext);

        byte[] encryptedBytes = MyAes.EncryptEcb(plaintextBytes, key);
        byte[] decryptedBytes = MyAes.DecryptEcb(encryptedBytes, key);

        string decryptedText = Encoding.ASCII.GetString(decryptedBytes);
        Assert.That(decryptedText, Is.EqualTo(plaintext));
    }
}