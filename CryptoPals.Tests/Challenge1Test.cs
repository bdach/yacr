using NUnit.Framework;

namespace CryptoPals.Tests
{
    // https://cryptopals.com/sets/1/challenges/1
    [TestFixture]
    public class Challenge1Test
    {
        [Test]
        public void TestHexToBase64Conversion()
        {
            const string hexString =
                "49276d206b696c6c696e6720796f757220627261696e206c696b65206120706f69736f6e6f7573206d757368726f6f6d";

            const string expectedBase64 = "SSdtIGtpbGxpbmcgeW91ciBicmFpbiBsaWtlIGEgcG9pc29ub3VzIG11c2hyb29t";

            Assert.That(hexString.FromHex().ToBase64(), Is.EqualTo(expectedBase64));
        }

        [Test]
        public void Base64ToHexConversion()
        {
            const string base64 = "SSdtIGtpbGxpbmcgeW91ciBicmFpbiBsaWtlIGEgcG9pc29ub3VzIG11c2hyb29t";

            const string expectedHexString =
                "49276d206b696c6c696e6720796f757220627261696e206c696b65206120706f69736f6e6f7573206d757368726f6f6d";

            Assert.That(base64.FromBase64().ToHex(), Is.EqualTo(expectedHexString));
        }

        [TestCase(new byte[] { 0x4D, 0x61, 0x6E }, "TWFu")]
        [TestCase(new byte[] { 0x4D, 0x61 }, "TWE=")]
        [TestCase(new byte[] { 0x4D }, "TQ==")]
        public void MoreBase64Conversions(byte[] bytes, string expectedBase64)
        {
            Assert.That(bytes.ToBase64(), Is.EqualTo(expectedBase64));
            Assert.That(expectedBase64.FromBase64(), Is.EquivalentTo(bytes));
        }
    }
}
