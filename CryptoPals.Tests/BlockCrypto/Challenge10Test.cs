using System.Text;
using NUnit.Framework;

namespace CryptoPals.Tests.BlockCrypto;

[TestFixture]
public class Challenge10Test
{
    [Test]
    public void TestAesEcbRoundTrip()
    {
        byte[] key = Encoding.ASCII.GetBytes("YELLOW SUBMARINE");

        const string plaintext = "This is 32 bytes of text, really";
        byte[] plaintextBytes = Encoding.ASCII.GetBytes(plaintext);
        byte[] iv = new byte[16];

        byte[] encryptedBytes = MyAes.EncryptCbc(plaintextBytes, key, iv);
        byte[] decryptedBytes = MyAes.DecryptCbc(encryptedBytes, key, iv);

        string decryptedText = Encoding.ASCII.GetString(decryptedBytes);
        Assert.That(decryptedText, Is.EqualTo(plaintext));
    }
}