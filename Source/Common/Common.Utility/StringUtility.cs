using System;
using System.Text;
using System.Text.RegularExpressions;

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

        public static string GenerateCaptchaCodeNum(int length)
        {
            int number;
            StringBuilder sbCaptchaCode = new StringBuilder(length);

            Random random = new Random(DateTime.Now.Millisecond);

            for (int i = 0; i < length; i++)
            {
                number = random.Next();

                sbCaptchaCode.Append((number % 10).ToString());
            }

            return sbCaptchaCode.ToString();
        }

        public static string GenerateCaptchaCode(int length)
        {
            int number;
            char tempCode;
            StringBuilder sbCaptchaCode = new StringBuilder(length);

            Random random = new Random(DateTime.Now.Millisecond);

            for (int i = 0; i < length; i++)
            {
                number = random.Next();

                if (random.Next() % 2 == 0)
                {
                    //number
                    tempCode = (char)('1' + (char)(number % 9));
                }
                else
                {
                    //letter
                    tempCode = (char)('A' + (char)(number % 26));
                }

                sbCaptchaCode.Append(tempCode);
            }

            return sbCaptchaCode.ToString();
        }

        /// <summary>
        /// 產生寛鬆規則的密碼值
        /// </summary>
        public static string GenerateLoosePasswordValue(int length)
        {
            int number;
            char tempCode;
            StringBuilder sbCaptchaCode = new StringBuilder(length);

            Random random = new Random(DateTime.Now.Millisecond);

            for (int i = 0; i < length; i++)
            {
                number = random.Next();

                if (random.Next() % 2 == 0)
                {
                    //number
                    tempCode = (char)('1' + (char)(number % 9));
                }
                else
                {
                    //letter
                    tempCode = (char)('A' + (char)(number % 26));
                }

                sbCaptchaCode.Append(tempCode);
            }

            return sbCaptchaCode.ToString();
        }

        /// <summary>
        /// 產生嚴格規則的密碼值
        /// </summary>
        public static string GenerateStrictPasswordValue(int length)
        {
            string symbols = "!@#$%^&*";
            int number;
            char tempCode;
            StringBuilder sbCaptchaCode = new StringBuilder(length);

            Random random = new Random(DateTime.Now.Millisecond);

            for (int i = 0; i < length; i++)
            {
                number = random.Next();

                if (random.Next() % 2 == 0)
                {
                    //number
                    tempCode = (char)('1' + (char)(number % 9));
                }
                else
                {
                    //letter
                    tempCode = (char)('A' + (char)(number % 26));
                }

                sbCaptchaCode.Append(tempCode);
            }

            int symbolIndex = random.Next() % symbols.Length;
            char symbol = symbols[symbolIndex];
            char lowerCaseLetter = (char)('a' + (char)(random.Next() % 26));

            sbCaptchaCode[0] = symbol;
            sbCaptchaCode[sbCaptchaCode.Length - 1] = lowerCaseLetter;

            return sbCaptchaCode.ToString();
        }

        /// <summary>
        /// 取得改寫上一層變數值用的js
        /// </summary>
        public static string GetWriteValueOfOpenerJs(string paraId, string value)
        {
            StringBuilder sbScript = new StringBuilder(300);

            if (value == "")
                value = " ";    //給上層判斷「未選擇」與「選擇到空字串」用

            sbScript.AppendLine("var myOpener;");
            sbScript.AppendLine("if(window.opener){ myOpener=window.opener; }");
            sbScript.AppendLine("  else if(window.parent){ myOpener=window.parent; }");
            sbScript.AppendLine("if(myOpener){");
            sbScript.AppendLine("  myOpener.document.getElementById('" + paraId + "').value='" + value + "';");
            sbScript.AppendLine("}");

            return sbScript.ToString();
        }

        /// <summary>
        /// 取得通知上一層用的js
        /// </summary>
        public static string GetNoticeOpenerJs(string flagValue)
        {
            string openderFormId = "form1";
            string flagId = "txtFlag";

            return GetNoticeOpenerJs(openderFormId, flagId, flagValue);
        }

        /// <summary>
        /// 取得通知上一層用的js
        /// </summary>
        public static string GetNoticeOpenerJs(string openderFormId, string flagId, string flagValue)
        {
            StringBuilder sbScript = new StringBuilder(300);

            sbScript.AppendLine("var myOpener;");
            sbScript.AppendLine("if(window.opener){ myOpener=window.opener; }");
            sbScript.AppendLine("  else if(window.parent){ myOpener=window.parent; }");
            sbScript.AppendLine("if(myOpener){");
            sbScript.AppendLine("  myOpener.document.getElementById('" + flagId + "').value='" + flagValue + "';");
            // 2017/11/07, lozen_lin, add, 修正上一個按鈕PostBack沒清掉事件造成submit()重覆呼叫上次事件的問題
            sbScript.AppendLine("if(myOpener.theForm){ var prForm = myOpener.theForm; prForm.__EVENTTARGET.value=''; prForm.__EVENTARGUMENT.value='' } ");
            // --
            sbScript.AppendLine("  myOpener.document.forms['" + openderFormId + "'].submit();");
            sbScript.AppendLine("  if(window.opener){ window.close(); }");
            sbScript.AppendLine("}");

            return sbScript.ToString();
        }

        public static string GetFormResizeJs(int w, int h)
        {
            StringBuilder sbScriptResize = new StringBuilder(200);

            sbScriptResize.AppendLine("$(function() {");
            sbScriptResize.AppendLine("  if(window.opener){");
            sbScriptResize.AppendFormat("  window.resizeTo({0}, {1});", w, h).AppendLine();
            sbScriptResize.AppendLine("  } else if(window.parent){");
            sbScriptResize.AppendFormat("  window.parent.resizeDialog({0}, {1});", w, h).AppendLine();
            sbScriptResize.AppendLine("  }");
            sbScriptResize.AppendLine("})");

            return sbScriptResize.ToString();
        }

        /// <summary>
        /// 取得簡單密碼規則的規則運算式
        /// </summary>
        public static string GetPswSimpleRuleValidationExpression()
        {
            return @"[a-zA-Z0-9`~!@#\$%\^&\*\(\)_\-\+=\{\}\[\]\\\|:;""'<>,\.\?/]{6,50}";
        }

        /// <summary>
        /// 取得過濾多餘資料的Html編輯器內文
        /// </summary>
        public static string GetSievedHtmlEditorValue(string htmlValue)
        {
            string managementDir = "Management/";

            if (htmlValue.IndexOf(managementDir, StringComparison.CurrentCultureIgnoreCase) >= 0)
            {
                htmlValue = Regex.Replace(htmlValue, "(?im)" + managementDir, "");
            }

            return htmlValue;
        }

        /// <summary>
        /// 取得語系狀態圖示的 css class
        /// </summary>
        public static string GetCssClassOfIconIsShowInLang(bool enabled)
        {
            if (enabled)
                return "badge badge-light text-info border border-info";
            else
                return "badge badge-light text-secondary";
        }

        /// <summary>
        /// 取得給下一層用的所有父層Num
        /// </summary>
        public static string GetNumOfParentsForChild(int Num, string NumOfParents)
        {
            string strResult = "";

            if (NumOfParents == "")
                strResult = Num.ToString();
            else
                strResult = NumOfParents + "," + Num.ToString();

            return strResult;
        }

        /// <summary>
        /// 取得給上一層用的所有父層Num
        /// </summary>
        public static string GetNumOfParentsForParent(string NumOfParents)
        {
            string strResult = "";
            int lastIndex = NumOfParents.LastIndexOf(',');

            if (lastIndex != -1)
                strResult = NumOfParents.Substring(0, lastIndex);

            return strResult;
        }

        /// <summary>
        /// 最後一個父層Num
        /// </summary>
        public static int GetLastNumOfParents(string NumOfParents)
        {
            int nResult;
            string strNum = NumOfParents;
            int lastIndex = NumOfParents.LastIndexOf(',');

            if (lastIndex != -1)
                strNum = NumOfParents.Substring(lastIndex).Replace(",", "");

            if (int.TryParse(strNum, out nResult) && nResult >= 1)
            {
            }
            else
                nResult = 1;

            return nResult;
        }

        /// <summary>
        /// 從網址找出 Youtube 影片代碼
        /// </summary>
        public static string GetYoutubeIdFromUrl(string url)
        {
            string youtubeId = "";
            string source = url;

            if (source.Contains("youtube.com"))
            {
                if (source.Contains("v="))
                {
                    //e.g., https://www.youtube.com/watch?v=FrM22iqsdpE

                    source = source.Split(new string[] { "v=" }, StringSplitOptions.RemoveEmptyEntries)[1];
                    youtubeId = source;
                }
                else if(source.Contains("/embed/"))
                {
                    //e.g., https://www.youtube.com/embed/FrM22iqsdpE

                    string[] tokens = source.Split('/');
                    source = tokens[tokens.Length - 1];
                    youtubeId = source;
                }
            }
            else if (source.Contains("youtu.be"))
            {
                //e.g., https://youtu.be/FrM22iqsdpE

                string[] tokens = source.Split('/');
                source = tokens[tokens.Length - 1];
                youtubeId = source;
            }

            if (source.Contains("?") || source.Contains("&") || source.Contains("#"))
            {
                //e.g., https://www.youtube.com/watch?v=FrM22iqsdpE&feature=youtu.be&t=150
                //e.g., https://www.youtube.com/embed/FrM22iqsdpE?start=120
                //e.g., https://youtu.be/FrM22iqsdpE?t=150

                source = source.Split('?', '&', '#')[0];
                youtubeId = source;
            }

            return youtubeId;
        }

        /// <summary>
        /// 取得經過開啟方式處理後的網址 Obtain ShowType processed URL
        /// </summary>
        public static string GetLinkUrlOfShowType(Guid articleId, int langNo, int showTypeId, string linkUrl)
        {
            string destUrl = "";

            if (showTypeId == 3 && linkUrl != "")   // showTypeId-3:URL
            {
                destUrl = linkUrl;

                if (destUrl.StartsWith("~/"))
                {
                    destUrl = destUrl.Replace("~/", "");
                }

                if (!linkUrl.StartsWith("http:", StringComparison.CurrentCultureIgnoreCase)
                    && !linkUrl.StartsWith("https:", StringComparison.CurrentCultureIgnoreCase))
                {
                    // inside page
                    destUrl = StringUtility.SetParaValueInUrl(destUrl, "artid", articleId.ToString());
                    destUrl = StringUtility.SetParaValueInUrl(destUrl, "l", langNo.ToString());
                }
            }
            else
            {
                destUrl = string.Format("Article.aspx?artid={0}&l={1}", articleId, langNo);
            }

            return destUrl;
        }
    }
}
