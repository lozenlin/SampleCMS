using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Common.Utility
{
    /// <summary>
    /// 使用 RNGCryptoServiceProvider 產生由密碼編譯服務供應者 (CSP) 提供的亂數產生器。
    /// 來源: http://blog.miniasp.com/post/2008/05/13/Random-vs-RNGCryptoServiceProvider.aspx
    /// </summary>
    public static class RNG
    {
        private static RNGCryptoServiceProvider rngp = new RNGCryptoServiceProvider();
        private static byte[] rb = new byte[4];

        static RNG()
        {
        }

        /// <summary>
        /// 產生一個非負數的亂數
        /// </summary>
        public static int Next()
        {
            rngp.GetBytes(rb);
            int value = BitConverter.ToInt32(rb, 0);
            if (value < 0) value = -value;

            return value;
        }

        /// <summary>
        /// 產生一個非負數且最大值 max 以下的亂數
        /// </summary>
        /// <param name="max">最大值</param>
        public static int Next(int max)
        {
            rngp.GetBytes(rb);
            int value = BitConverter.ToInt32(rb, 0);
            value = value % (max + 1);
            if (value < 0) value = -value;

            return value;
        }

        /// <summary>
        /// 產生一個非負數且最小值在 min 以上最大值在 max 以下的亂數
        /// </summary>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <returns></returns>
        public static int Next(int min, int max)
        {
            int value = Next(max - min) + min;

            return value;
        }
    }
}
