using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace Common.Utility
{
    /// <summary>
    /// DesUtility 加密解密
    /// </summary>
    public class DesUtility
    {
        protected DesUtility()
        {
        }

        public static string Decrypt(string hexString, string key8B, string iv8B)
        {
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider
            {
                Key = Encoding.ASCII.GetBytes(key8B),
                IV = Encoding.ASCII.GetBytes(iv8B)
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

        public static string Encrypt(string original, string key8B, string iv8B)
        {
            DESCryptoServiceProvider provider = new DESCryptoServiceProvider
            {
                Key = Encoding.ASCII.GetBytes(key8B),
                IV = Encoding.ASCII.GetBytes(iv8B)
            };
            byte[] bytes = Encoding.UTF8.GetBytes(original);
            ICryptoTransform encryptor = provider.CreateEncryptor();
            byte[] encryptedData = encryptor.TransformFinalBlock(bytes, 0, bytes.Length);

            return BitConverter.ToString(encryptedData).Replace("-", string.Empty);
        }

    }
}
