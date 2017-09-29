﻿using System.Text;

namespace Common.Utility
{
    /// <summary>
    /// 字串處理
    /// </summary>
    public class StringUtility
    {
        protected StringUtility()
        {
        }

        /// <summary>
        /// 移除文字裡的所有 Html 標籤
        /// </summary>
        public static string RemoveHtmlTag(string html)
        {
            HtmlTextHandler textHandler = new HtmlTextHandler();

            return textHandler.StripTagsCharArray(html);
        }

        /// <summary>
        /// 移除文字裡的所有 Html 標籤與空白換行
        /// </summary>
        public static string RemoveHtmlTagWoEmptyLines(string html)
        {
            HtmlTextHandler textHandler = new HtmlTextHandler();
            string result = textHandler.StripTagsCharArray(html);

            //拿掉換行符號,開頭,間隔的一堆空白
            result = System.Text.RegularExpressions.Regex.Replace(result, @"\s+", " ");

            return result;
        }

        /// <summary>
        /// 移除文字裡的所有 Html 標籤,並截短內文
        /// </summary>
        public static string RemoveHtmlTagToShortContext(string context, int maxLength)
        {
            //移除Html碼
            string shortArticleContext = RemoveHtmlTagWoEmptyLines(context);

            //節錄為簡介
            if (shortArticleContext.Length > maxLength)
                shortArticleContext = shortArticleContext.Remove(0, shortArticleContext.Length - maxLength) + "...";

            return shortArticleContext;
        }

        /// <summary>
        /// 設定網址中的參數值
        /// </summary>
        public static string SetParaValueInUrl(string url, string paraName, string paraValue)
        {
            StringBuilder sbResult = new StringBuilder();
            //檢查原內容是否有帶參數
            bool HasPara = false;
            if (url.IndexOf("?") != -1)
                HasPara = true;

            if (HasPara)
            {
                //檢查原內容是否有指定參數
                int SplitterIndexOfPara = url.ToLower().IndexOf("?" + paraName.ToLower() + "=");
                //第一個不是,再找之後的
                if (SplitterIndexOfPara == -1)
                    SplitterIndexOfPara = url.ToLower().IndexOf("&" + paraName.ToLower() + "=");

                if (SplitterIndexOfPara != -1)
                {
                    //已有指定參數
                    int startIndex = SplitterIndexOfPara + 1;
                    int NextSplitterIndex = url.ToLower().IndexOf("&", startIndex);

                    if (NextSplitterIndex == -1)
                    {
                        //原指定參數是最後一個
                        //移掉原指定參數,重加
                        sbResult.Append(url);
                        sbResult.Remove(startIndex, sbResult.Length - startIndex);
                        sbResult.AppendFormat("{0}={1}", paraName, paraValue);
                    }
                    else
                    {
                        //原指定參數在中間
                        //先加前面的部分
                        sbResult.Append(url.Substring(0, SplitterIndexOfPara + paraName.Length + 2/*p的前後符號*/));
                        //再加指定參數
                        sbResult.Append(paraValue);
                        //再加後面部分
                        sbResult.Append(url.Substring(NextSplitterIndex));
                    }
                }
                else
                {
                    //沒有指定參數
                    sbResult.AppendFormat("{0}&{1}={2}", url, paraName, paraValue);
                }
            }
            else
            {
                //原內容沒參數
                sbResult.AppendFormat("{0}?{1}={2}", url, paraName, paraValue);
            }

            return sbResult.ToString();
        }

        /// <summary>
        /// 加入時間冒號
        /// </summary>
        public static string InsertTimeColon(string hourMinSec)
        {
            string result = "";
            //HHmm HHmmss
            int iHourColon = 2;
            //Hmm Hmmss
            if (hourMinSec.Length % 2 != 0)
                iHourColon = 1;

            if (hourMinSec.Length >= iHourColon)
                result = hourMinSec.Insert(iHourColon, ":");

            if (hourMinSec.Length > iHourColon + 2)
                result = result.Insert(iHourColon + 2 + 1, ":");

            return result;
        }

    }
}
