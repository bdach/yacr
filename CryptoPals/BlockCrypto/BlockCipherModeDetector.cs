using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace CryptoPals.BlockCrypto;

public class BlockCipherModeDetector
{
    public CipherMode Detect(IEncryptionOracle oracle)
    {
        /*
         * let's be crafty here and stuff the plaintext with the same byte over and over.
         * the length of the plaintext is going to be triple the key size.
         *
         * now the reason why it's triple is simple:
         * we know that we can see through ECB.
         * in ECB same input => same output. so we want to see some repeating output from the oracle.
         * but the task said to pad the output with random bytes at the start and end.
         * depending on luck, this may mess with the block offset and move our stuffed A's around.
         * therefore we generate three blocks, so that there's no conceivable way we can't hit 2 identical ones.
         */
        var plaintext = new string(Enumerable.Repeat('A', 3 * oracle.KeySize).ToArray());
        byte[] plaintextBytes = Encoding.ASCII.GetBytes(plaintext);

        byte[] ciphertext = oracle.Encrypt(plaintextBytes);

        // now, check neighbouring blocks.
        // a repeat is an indicator of a successful ECB hit.
        int blockCount = ciphertext.Length / oracle.KeySize;

        for (var i = 0; i < blockCount - 1; ++i)
        {
            byte[] firstBlock = ciphertext[(i * oracle.KeySize)..((i + 1) * oracle.KeySize)];
            byte[] secondBlock = ciphertext[((i + 1) * oracle.KeySize)..((i + 2) * oracle.KeySize)];

            if (firstBlock.SequenceEqual(secondBlock))
                return CipherMode.ECB;
        }

        return CipherMode.CBC;
    }
}