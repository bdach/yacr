namespace CryptoPals.Basics.XOR;

public static class RepeatingKeyXOR
{
    public static byte[] Combine(byte[] plaintext, byte[] key)
    {
        byte[] repeatingKey = new byte[plaintext.Length];

        for (int i = 0; i < repeatingKey.Length; ++i)
        {
            repeatingKey[i] = key[i % key.Length];
        }

        return FixedXOR.Combine(plaintext, repeatingKey);
    }
}