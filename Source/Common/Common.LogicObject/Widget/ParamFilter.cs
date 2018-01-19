// ===============================================================================
// ParamFilter of SampleCMS
// https://github.com/lozenlin/SampleCMS
//
// ParamFilter.cs
//
// ===============================================================================
// Copyright (c) 2018 lozenlin
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// ===============================================================================

using log4net;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Common.LogicObject
{
    /// <summary>
    /// 參數過濾
    /// </summary>
    public abstract class ParamFilter
    {
        protected ParamFilter successor;
        protected ILog logger;

        public ParamFilter()
        {
            logger = LogManager.GetLogger(this.GetType());
        }

        protected void ShowDebugMsg(ParamInfo paramInfo)
        {
            logger.DebugFormat("key[{0}] value[{1}] of page[{2}]", paramInfo.Key, paramInfo.Value, paramInfo.ExecFilePath);
        }

        protected void ShowInfoMsgBeforeFailed(ParamInfo paramInfo)
        {
            logger.InfoFormat("key[{0}] value[{1}] of page[{2}] Failed!", paramInfo.Key, paramInfo.Value, paramInfo.ExecFilePath);
        }

        public void SetSuccessor(ParamFilter successor)
        {
            this.successor = successor;
        }

        public abstract bool HandleRequest(ParamInfo paramInfo);

        /// <summary>
        /// 參數資訊
        /// </summary>
        public class ParamInfo
        {
            /// <summary>
            /// Key (will use lowercase)
            /// </summary>
            public string Key
            {
                get { return key; }
                set { key = (value == null) ? "" : value.ToLower(); }
            }
            private string key;

            public string Value;

            /// <summary>
            /// ExecFilePath (will use lowercase)
            /// </summary>
            public string ExecFilePath
            {
                get { return execFilePath; }
                set { execFilePath = (value == null) ? "" : value.ToLower(); }
            }
            private string execFilePath;
        }
    }

    /// <summary>
    /// 非字串參數過濾
    /// </summary>
    public class NonStringParamFilter : ParamFilter
    {
        /// <summary>
        /// Int32 類型的參數名單 (needs lowercase)
        /// </summary>
        private List<string> intParamList;

        /// <summary>
        /// 日期類型的參數名單 (needs lowercase)
        /// </summary>
        private List<string> dateTimeParamList;

        /// <summary>
        /// Guid 類型的參數名單 (needs lowercase)
        /// </summary>
        private List<string> guidParamList;

        /// <summary>
        /// 非字串參數過濾
        /// </summary>
        public NonStringParamFilter()
            : base()
        {
        }

        /// <summary>
        /// 指定 Int32 類型的參數名單 (needs lowercase)
        /// </summary>
        public void SetIntParamList(List<string> paramList)
        {
            intParamList = paramList;
        }

        /// <summary>
        /// 指定日期類型的參數名單 (needs lowercase)
        /// </summary>
        public void SetDateTimeParamList(List<string> paramList)
        {
            dateTimeParamList = paramList;
        }

        /// <summary>
        /// 指定 Guid 類型的參數名單 (needs lowercase)
        /// </summary>
        public void SetGuidParamList(List<string> paramList)
        {
            guidParamList = paramList;
        }

        public override bool HandleRequest(ParamInfo paramInfo)
        {
            ShowDebugMsg(paramInfo);

            if (paramInfo.Value == null)
                return true;

            bool result = false;

            //檢查Int32類型
            if (intParamList != null && intParamList.Contains(paramInfo.Key))
            {
                int nResult;
                result = int.TryParse(paramInfo.Value, out nResult);

                if (!result)
                {
                    ShowInfoMsgBeforeFailed(paramInfo);
                }

                return result;
            }

            //檢查日期類型
            if (dateTimeParamList != null && dateTimeParamList.Contains(paramInfo.Key))
            {
                DateTime dtResult;
                result = DateTime.TryParse(paramInfo.Value, out dtResult);

                if (!result)
                {
                    ShowInfoMsgBeforeFailed(paramInfo);
                }
                return result;

            }

            //檢查 Guid 類型
            if (guidParamList != null && guidParamList.Contains(paramInfo.Key))
            {
                Guid guidResult;
                result = Guid.TryParse(paramInfo.Value, out guidResult);

                if (!result)
                {
                    ShowInfoMsgBeforeFailed(paramInfo);
                }

                return result;
            }

            if (successor == null)
                return true;

            //換下一個參數過濾物件檢查
            return successor.HandleRequest(paramInfo);
        }
    }

    /// <summary>
    /// 有限制長度的字串參數過濾
    /// </summary>
    public class LimitedStringParamFilter : ParamFilter
    {
        /// <summary>
        /// 參數名稱與內容長度對照表
        /// </summary>
        private Dictionary<string, int> paramValueLenLookup;

        /// <summary>
        /// 有限制長度的字串參數過濾
        /// </summary>
        public LimitedStringParamFilter()
            : base()
        {
        }

        /// <summary>
        /// 指定參數名稱與內容長度對照表 (needs lowercase)
        /// </summary>
        public void SetParamValueLenLookup(Dictionary<string, int> lenLookup)
        {
            paramValueLenLookup = lenLookup;
        }

        public override bool HandleRequest(ParamInfo paramInfo)
        {
            ShowDebugMsg(paramInfo);

            if (paramInfo.Value == null)
                return true;

            //檢查字串長度
            if (paramValueLenLookup != null && paramValueLenLookup.ContainsKey(paramInfo.Key))
            {
                //超過指定長度
                if (paramInfo.Value.Length > paramValueLenLookup[paramInfo.Key])
                {
                    ShowInfoMsgBeforeFailed(paramInfo);
                    return false;
                }
            }

            //讓較短的先回報有效,太長的還是往下一個送
            if (paramInfo.Value.Length <= 5)
                return true;

            if (successor == null)
                return true;

            //換下一個參數過濾物件檢查
            return successor.HandleRequest(paramInfo);
        }
    }

    /// <summary>
    /// 黑名單關鍵字過濾
    /// </summary>
    public class BlacklistKeywordFilter : ParamFilter
    {
        /// <summary>
        /// 黑名單
        /// </summary>
        private string[] blacklistKeywords;

        /// <summary>
        /// 黑名單關鍵字在特定頁面中要允許的白名單
        /// </summary>
        private Dictionary<string, NameValueCollection> whitelistOfBlacklistKeywords;

        /// <summary>
        /// 黑名單關鍵字過濾
        /// </summary>
        public BlacklistKeywordFilter()
            : base()
        {
        }

        /// <summary>
        /// 指定黑名單
        /// </summary>
        public void SetBlacklistKeywords(string[] keywords)
        {
            blacklistKeywords = keywords;
        }

        /// <summary>
        /// 指定黑名單關鍵字在特定頁面中要允許的白名單
        /// ( 頁面名[需小寫] -> 關鍵字 -> ,變數名,變數名, )
        /// ( execFilePath[needs lowercase] -> Keyword -> ,ParamName,ParamName, )
        /// </summary>
        public void SetWhitelistOfBlacklistKeywords(Dictionary<string, NameValueCollection> whitelistOfBlacklistKeywords)
        {
            this.whitelistOfBlacklistKeywords = whitelistOfBlacklistKeywords;
        }

        public override bool HandleRequest(ParamInfo paramInfo)
        {
            ShowDebugMsg(paramInfo);

            if (paramInfo.Value == null)
                return true;

            //拿掉空白後再檢查關鍵字
            string noSpaceValue = paramInfo.Value.Replace(" ", "");

            if (blacklistKeywords != null)
            {
                foreach (string kw in blacklistKeywords)
                {
                    //檢查白名單
                    if (whitelistOfBlacklistKeywords != null && whitelistOfBlacklistKeywords.ContainsKey(paramInfo.ExecFilePath))
                    {
                        //取得以逗號相接的變數名單
                        string whitelist = whitelistOfBlacklistKeywords[paramInfo.ExecFilePath][kw];

                        //在白名單內的跳過
                        if (whitelist != null && whitelist.Contains("," + paramInfo.Key + ","))
                            continue;
                    }

                    if (noSpaceValue.IndexOf(kw, StringComparison.CurrentCultureIgnoreCase) != -1)
                    {
                        ShowInfoMsgBeforeFailed(paramInfo);
                        return false;
                    }
                }
            }

            if (successor == null)
                return true;

            //換下一個參數過濾物件檢查
            return successor.HandleRequest(paramInfo);
        }
    }

    /// <summary>
    /// 用 UrlDecode 解碼參數內容
    /// </summary>
    public class UrlDecodeParamValue : ParamFilter
    {
        /// <summary>
        /// 需清除的字串名單
        /// </summary>
        private string[] dirtyStringArray = new string[] { "%09", "%0d", "%0a" };

        /// <summary>
        /// 用 UrlDecode 解碼參數內容
        /// </summary>
        public UrlDecodeParamValue()
            : base()
        {
        }

        public override bool HandleRequest(ParamInfo paramInfo)
        {
            ShowDebugMsg(paramInfo);

            if (paramInfo.Value == null)
                return true;

            //先處理需清除的字串
            StringBuilder sbValue = new StringBuilder(paramInfo.Value);

            foreach (string dirtyString in dirtyStringArray)
            {
                sbValue.Replace(dirtyString, "");
            }

            //解碼
            paramInfo.Value = HttpUtility.UrlDecode(sbValue.ToString());

            //沒下一個時,預設搜尋黑名單關鍵字
            if (successor == null)
                successor = new BlacklistKeywordFilter();

            //換下一個參數過濾物件檢查
            return successor.HandleRequest(paramInfo);
        }
    }

    /// <summary>
    /// 用 HtmlDecode 解碼參數內容
    /// </summary>
    public class HtmlDecodeParamValue : ParamFilter
    {
        /// <summary>
        /// 需清除的字串名單
        /// </summary>
        private string[] dirtyStringArray = new string[] { "\t", "\r", "\n" };

        /// <summary>
        /// 用 HtmlDecode 解碼參數內容
        /// </summary>
        public HtmlDecodeParamValue()
            : base()
        {
        }

        public override bool HandleRequest(ParamInfo paramInfo)
        {
            ShowDebugMsg(paramInfo);

            if (paramInfo.Value == null)
                return true;

            string translatedValue = paramInfo.Value;

            //將 &#x6a 翻譯為 &#x6a;  &#106 翻譯為 &#106; (加上分號[;])
            string pattern = @"(?is)(?<prefix>&#)(?<cnt>[^\s&#\\;]+);?";
            string replacement = @"&#${cnt};";

            if (Regex.IsMatch(translatedValue, pattern))
            {
                translatedValue = Regex.Replace(translatedValue, pattern, replacement);
            }

            //將 \x6a 翻譯為 &#x6a;  \75 翻譯為 &#x75;
            pattern = @"(?is)(?<prefix>\\x?)(?<cnt>[^\s&#\\;]+)";
            replacement = @"&#x${cnt};";

            if (Regex.IsMatch(translatedValue, pattern))
            {
                translatedValue = Regex.Replace(translatedValue, pattern, replacement);
            }

            //解碼
            string decodeValue = HttpUtility.HtmlDecode(translatedValue);

            //處理需清除的字串
            StringBuilder sbValue = new StringBuilder(decodeValue);

            foreach (string dirtyString in dirtyStringArray)
            {
                sbValue.Replace(dirtyString, "");
            }

            paramInfo.Value = sbValue.ToString();

            //沒下一個時,預設搜尋黑名單關鍵字
            if (successor == null)
                successor = new BlacklistKeywordFilter();

            //換下一個參數過濾物件檢查
            return successor.HandleRequest(paramInfo);
        }
    }

    /// <summary>
    /// 針對 Acunetix 送來的 Post 參數過濾
    /// </summary>
    public class ForAcunetixPostParamFilter : ParamFilter
    {
        /// <summary>
        /// 針對 Acunetix 送來的 Post 參數過濾
        /// </summary>
        public ForAcunetixPostParamFilter()
            : base()
        {
        }

        public override bool HandleRequest(ParamInfo paramInfo)
        {
            ShowDebugMsg(paramInfo);

            // Acunetix 把 ctl00$contentplaceholder1$uclogin$txtAccount 改用 ctl00%24contentplaceholder1%24uclogin%24txtAccount 送來
            if (paramInfo.Key.IndexOf("%24") != -1)
            {
                ShowInfoMsgBeforeFailed(paramInfo);
                return false;
            }

            if (successor == null)
                return true;

            //換下一個參數過濾物件檢查
            return successor.HandleRequest(paramInfo);
        }
    }

    /// <summary>
    /// 特殊頁面參數過濾
    /// </summary>
    public class SpecificPageParamFilter : ParamFilter
    {
        /// <summary>
        /// 特殊頁面參數過濾
        /// </summary>
        public SpecificPageParamFilter()
            : base()
        {
        }

        public override bool HandleRequest(ParamInfo paramInfo)
        {
            ShowDebugMsg(paramInfo);

            if (paramInfo.Value == null)
                return true;

            string pattern = "";
            MatchCollection matches = null;

            switch (paramInfo.ExecFilePath)
            {
                case "scriptresource.axd":
                case "webresource.axd":
                    switch (paramInfo.Key)
                    {
                        case "t":
                            pattern = @"[\da-fA-f]+";    //長短不一定的十六進制數字
                            matches = Regex.Matches(paramInfo.Value, pattern);

                            //要全字串符合
                            if (matches.Count != 1 || matches[0].Value != paramInfo.Value)
                            {
                                ShowInfoMsgBeforeFailed(paramInfo);
                                return false;
                            }

                            //不需要再送其他檢查
                            return true;
                        case "d":
                            pattern = @"[A-Za-z0-9+/=_\-]+";    //比Base-64多了_-
                            matches = Regex.Matches(paramInfo.Value, pattern);

                            //要全字串符合
                            if (matches.Count != 1 || matches[0].Value != paramInfo.Value)
                            {
                                ShowInfoMsgBeforeFailed(paramInfo);
                                return false;
                            }

                            //已知最短長度
                            if (paramInfo.Value.Length < 16)
                            {
                                ShowInfoMsgBeforeFailed(paramInfo);
                                return false;
                            }

                            break;
                    }
                    break;
            }


            if (successor == null)
                return true;

            //換下一個參數過濾物件檢查
            return successor.HandleRequest(paramInfo);
        }
    }

    /// <summary>
    /// 規則表達式黑名單過濾
    /// </summary>
    public class RegexParamFilter : ParamFilter
    {
        /// <summary>
        /// 規則表達式黑名單
        /// </summary>
        private string[] blacklistPatterns;

        /// <summary>
        /// 黑名單在特定頁面中要允許的白名單
        /// </summary>
        private Dictionary<string, NameValueCollection> whiteListOfBlacklistPatterns;

        /// <summary>
        /// 規則表達式黑名單過濾
        /// </summary>
        public RegexParamFilter()
            : base()
        {
        }

        /// <summary>
        /// 指定黑名單
        /// </summary>
        public void SetBlacklistPatterns(string[] patterns)
        {
            blacklistPatterns = patterns;
        }

        /// <summary>
        /// 指定黑名單在特定頁面中要允許的白名單
        /// ( 頁面名[需小寫] -> 規則 -> ,變數名,變數名, )
        /// ( execFilePath[needs lowercase] -> Pattern -> ,ParamName,ParamName, )
        /// </summary>
        public void SetWhitelistOfBlacklistPatterns(Dictionary<string, NameValueCollection> whitelistOfBlacklistPatterns)
        {
            this.whiteListOfBlacklistPatterns = whitelistOfBlacklistPatterns;
        }

        public override bool HandleRequest(ParamInfo paramInfo)
        {
            ShowDebugMsg(paramInfo);

            if (paramInfo.Value == null || paramInfo.Value.Length > 1000)   //hack, 太長的字有效能問題,先避開
            {
                if (successor == null)
                    return true;

                //換下一個參數過濾物件檢查
                return successor.HandleRequest(paramInfo);
            }

            if (blacklistPatterns != null)
            {
                foreach (string pattern in blacklistPatterns)
                {
                    //檢查白名單
                    if (whiteListOfBlacklistPatterns != null && whiteListOfBlacklistPatterns.ContainsKey(paramInfo.ExecFilePath))
                    {
                        //取得以逗號相接的變數名單
                        string whiteList = whiteListOfBlacklistPatterns[paramInfo.ExecFilePath][pattern];

                        //在白名單內的跳過
                        if (whiteList != null && whiteList.Contains("," + paramInfo.Key + ","))
                            continue;
                    }

                    if (Regex.IsMatch(paramInfo.Value, pattern))
                    {
                        ShowInfoMsgBeforeFailed(paramInfo);
                        return false;
                    }
                }
            }

            if (successor == null)
                return true;

            //換下一個參數過濾物件檢查
            return successor.HandleRequest(paramInfo);
        }
    }

    /// <summary>
    /// SQL Injection 過濾
    /// </summary>
    public class SQLInjectionFilter : ParamFilter
    {
        /// <summary>
        /// SQL Injection 過濾
        /// </summary>
        public SQLInjectionFilter()
            : base()
        {
        }

        public override bool HandleRequest(ParamInfo paramInfo)
        {
            ShowDebugMsg(paramInfo);

            if (paramInfo.Value == null)
                return true;

            string pattern = @"(?is)(?<verb>and|or)\s+(?<expr>.+?\s*=\s*['""]?\s*[^\s-;'""]+)";    //不考慮編碼 or xxx=xxx 或 xxx' and 31337-31337='0
            string verb, expr;

            //檢查是否有 SQL Injection 的關鍵字
            foreach (Match match in Regex.Matches(paramInfo.Value, pattern))
            {
                verb = match.Groups["verb"].Value;
                expr = match.Groups["expr"].Value;

                //測試運算式是否成立,能否被用來做 SQL Injection
                if (IsSQLInjectionExpr(expr))
                {
                    ShowInfoMsgBeforeFailed(paramInfo);
                    return false;
                }
            }

            if (successor == null)
                return true;

            //換下一個參數過濾物件檢查
            return successor.HandleRequest(paramInfo);
        }

        /// <summary>
        /// 測試運算式是否成立,能否被用來做 SQL Injection
        /// </summary>
        protected virtual bool IsSQLInjectionExpr(string expr)
        {
            //不提供測試時,直接當做有問題
            return true;
        }
    }
}
