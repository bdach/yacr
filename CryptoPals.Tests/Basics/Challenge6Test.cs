using System.Text;
using CryptoPals.Basics;
using NUnit.Framework;

namespace CryptoPals.Tests.Basics;

[TestFixture]
public class Challenge6Test
{
    [TestCase("this is a test", "wokka wokka!!!", 37)]
    public void TestHammingDistance(string first, string second, int expectedDistance)
    {
        byte[] firstBytes = Encoding.ASCII.GetBytes(first);
        byte[] secondBytes = Encoding.ASCII.GetBytes(second);

        Assert.That(HammingDistance.Calculate(firstBytes, secondBytes), Is.EqualTo(expectedDistance));
    }
}