using System;
using System.Security.Cryptography;
using System.Text;

namespace Common.Utility
{
    /// <summary>
    /// 雜湊資料
    /// </summary>
    public class HashUtility
    {
        protected HashUtility()
        {
        }

        /// <summary>
        /// 取得指定方法的雜湊結果
        /// </summary>
        public static string GetHashOf(string hashName, string value)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(value);
            HashAlgorithm hashAlgorithm = HashAlgorithm.Create(hashName);
            byte[] hashedData = hashAlgorithm.ComputeHash(bytes);

            return BitConverter.ToString(hashedData).Replace("-", "");
        }

        /// <summary>
        /// 取得用 SHA1 的雜湊結果
        /// </summary>
        public static string GetHashOfSHA1(string value)
        {
            return GetHashOf("SHA1", value);
        }

        /// <summary>
        /// 取得用 SHA256 的雜湊結果
        /// </summary>
        public static string GetHashOfSHA256(string value)
        {
            return GetHashOf("SHA256", value);
        }

        /// <summary>
        /// 取得用 SHA512 的雜湊結果
        /// </summary>
        public static string GetHashOfSHA512(string value)
        {
            return GetHashOf("SHA512", value);
        }

        /// <summary>
        /// 取得用 MD5 的雜湊結果
        /// </summary>
        public static string GetHashOfMD5(string value)
        {
            return GetHashOf("MD5", value);
        }

    }
}
