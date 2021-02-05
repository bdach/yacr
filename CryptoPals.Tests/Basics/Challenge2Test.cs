using CryptoPals.Basics.Conversions;
using CryptoPals.Basics.XOR;
using NUnit.Framework;

namespace CryptoPals.Tests.Basics
{
    // https://cryptopals.com/sets/1/challenges/2
    [TestFixture]
    public class Challenge2Test
    {
        [Test]
        public void TestFixedXOR()
        {
            const string input = "1c0111001f010100061a024b53535009181c";
            const string key = "686974207468652062756c6c277320657965";

            const string expectedOutput = "746865206b696420646f6e277420706c6179";

            Assert.That(FixedXOR.Combine(input.FromHex(), key.FromHex()).ToHex(), Is.EqualTo(expectedOutput));
        }
    }
}
