using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace Common.Utility
{
    /// <summary>
    /// AES 加密解密
    /// </summary>
    public class AesUtility
    {
        protected AesUtility()
        {
        }

        public static string Decrypt(string hexString, string key16B, string iv16B)
        {
            AesCryptoServiceProvider provider = new AesCryptoServiceProvider
            {
                Key = Encoding.ASCII.GetBytes(key16B),
                IV = Encoding.ASCII.GetBytes(iv16B)
            };
            byte[] inputBuffer = new byte[hexString.Length / 2];
            int num = 0;

            for (int i = 0; i < (hexString.Length / 2); i++)
            {
                inputBuffer[i] = byte.Parse(hexString[num].ToString() + hexString[num + 1], NumberStyles.HexNumber);
                num += 2;
            }

            ICryptoTransform decryptor = provider.CreateDecryptor();
            byte[] decryptedData = decryptor.TransformFinalBlock(inputBuffer, 0, inputBuffer.Length);

            return Encoding.UTF8.GetString(decryptedData);
        }

        public static string Encrypt(string original, string key16B, string iv16B)
        {
            AesCryptoServiceProvider provider = new AesCryptoServiceProvider
            {
                Key = Encoding.ASCII.GetBytes(key16B),
                IV = Encoding.ASCII.GetBytes(iv16B)
            };
            byte[] bytes = Encoding.UTF8.GetBytes(original);
            ICryptoTransform encryptor = provider.CreateEncryptor();
            byte[] enctyptedData = encryptor.TransformFinalBlock(bytes, 0, bytes.Length);

            return BitConverter.ToString(enctyptedData).Replace("-", string.Empty);
        }

    }
}
