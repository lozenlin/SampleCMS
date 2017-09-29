using Microsoft.VisualBasic;
using System.Collections.Generic;

namespace Common.Utility
{
    /// <summary>
    /// 語言相關功能
    /// </summary>
    public class LangUtility
    {
        protected LangUtility()
        {
        }

        /*
         * references:
         * https://dotblogs.com.tw/atalin/2010/11/04/18799
         * https://msdn.microsoft.com/en-us/library/ms912047(WinEmbedded.10).aspx
         */

        /// <summary>
        /// 繁體中文轉為簡體中文
        /// </summary>
        public static string ConvertToSimplifiedChinese(string traditionalChinese)
        {
            return Strings.StrConv(traditionalChinese, VbStrConv.SimplifiedChinese, 2052/*People's Republic of China*/);
        }

        /// <summary>
        /// 簡體中文轉為繁體中文
        /// </summary>
        public static string ConvertToTraditionalChinese(string simplifiedChinese)
        {
            return Strings.StrConv(simplifiedChinese, VbStrConv.TraditionalChinese, 1028/*Taiwan*/);
        }

        /// <summary>
        /// 將字串中每個字的第一個字母轉換為大寫
        /// </summary>
        public static string ConvertToProperCase(string value)
        {
            return Strings.StrConv(value, VbStrConv.ProperCase);
        }

        /// <summary>
        /// 將字串中的半形 (單一位元組) 字元轉換為全形 (雙位元組) 字元
        /// </summary>
        public static string ConvertToWide(string value)
        {
            return Strings.StrConv(value, VbStrConv.Wide);
        }

        /// <summary>
        /// 將字串中的全形 (雙位元組) 字元轉換為半形 (單一位元組) 字元
        /// </summary>
        public static string ConvertToNarrow(string value)
        {
            return Strings.StrConv(value, VbStrConv.Narrow);
        }

        /// <summary>
        /// 取得字串裡的難字
        /// </summary>
        /// <param name="value">來源值</param>
        /// <param name="newCJKs">WinXP 尚未支援的文字</param>
        /// <param name="custCJKs">造字區的文字</param>
        public static void GetNewChineseWordOf(string value, out List<string> newCJKs, out List<string> custCJKs)
        {
            newCJKs = new List<string>();
            custCJKs = new List<string>();

            if (string.IsNullOrEmpty(value))
                return;

            //字碼範圍
            // WinXP 尚未支援的中文
            int cjkExtAMin = 0x3400;
            int cjkExtAMax = 0x4DFF;

            // 造字區的 unicode 範圍
            int custCjkMin = 0xE000;
            int custCjkMax = 0xF8FF;

            char[] charArray = value.ToCharArray();

            foreach (char charValue in charArray)
            {
                if (cjkExtAMin <= charValue
                    && charValue <= cjkExtAMax)
                {
                    newCJKs.Add(charValue.ToString());
                }
                else if (custCjkMin <= charValue
                    && charValue <= custCjkMax)
                {
                    custCJKs.Add(charValue.ToString());
                }
            }
        }

    }
}
