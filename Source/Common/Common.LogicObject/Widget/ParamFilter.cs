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
using System.Threading.Tasks;
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

        protected void ShowDebugInfo(ParamInfo paramInfo)
        {
            logger.DebugFormat("key[{0}] value[{1}] of page[{2}]", paramInfo.Key, paramInfo.Value, paramInfo.ExecFilePath);
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
            public string Key;
            public string Value;
            public string ExecFilePath;
        }
    }

    /// <summary>
    /// 非字串參數過濾
    /// </summary>
    public class NonStringParamFilter : ParamFilter
    {
        /// <summary>
        /// Int32 類型的參數名單
        /// </summary>
        private List<string> intParamList;

        /// <summary>
        /// 日期類型的參數名單
        /// </summary>
        private List<string> dateTimeParamList;

        /// <summary>
        /// Guid 類型的參數名單
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
        /// 指定 Int32 類型的參數名單
        /// </summary>
        public void SetIntParamList(List<string> paramList)
        {
            intParamList = paramList;
        }

        /// <summary>
        /// 指定日期類型的參數名單
        /// </summary>
        public void SetDateTimeParamList(List<string> paramList)
        {
            dateTimeParamList = paramList;
        }

        /// <summary>
        /// 指定 Guid 類型的參數名單
        /// </summary>
        public void SetGuidParamList(List<string> paramList)
        {
            guidParamList = paramList;
        }

        public override bool HandleRequest(ParamInfo paramInfo)
        {
            ShowDebugInfo(paramInfo);

            if (paramInfo.Value == null)
                return true;

            //檢查Int32類型
            if (intParamList != null && intParamList.Any<string>(x => string.Compare(x, paramInfo.Key, true) == 0))
            {
                int nResult;
                return int.TryParse(paramInfo.Value, out nResult);
            }

            //檢查日期類型
            if (dateTimeParamList != null && dateTimeParamList.Any<string>(x => string.Compare(x, paramInfo.Key, true) == 0))
            {
                DateTime dtResult;
                return DateTime.TryParse(paramInfo.Value, out dtResult);
            }

            //檢查 Guid 類型
            if (guidParamList != null && guidParamList.Any<string>(x => string.Compare(x, paramInfo.Key, true) == 0))
            {
                Guid guidResult;
                return Guid.TryParse(paramInfo.Value, out guidResult);
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
        /// 指定參數名稱與內容長度對照表
        /// </summary>
        public void SetParamValueLenLookup(Dictionary<string, int> lenLookup)
        {
            paramValueLenLookup = lenLookup;
        }

        public override bool HandleRequest(ParamInfo paramInfo)
        {
            ShowDebugInfo(paramInfo);

            if (paramInfo.Value == null)
                return true;

            //檢查字串長度
            if (paramValueLenLookup != null && paramValueLenLookup.Keys.Any(x => string.Compare(x, paramInfo.Key) == 0))
            {
                //超過指定長度
                if (paramInfo.Value.Length > paramValueLenLookup[paramInfo.Key])
                    return false;
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
    public class BlackKeyWordFilter : ParamFilter
    {
        /// <summary>
        /// 黑名單
        /// </summary>
        private string[] blackKeyWords;

        /// <summary>
        /// 黑名單關鍵字在特定頁面中要允許的白名單
        /// </summary>
        private Dictionary<string, NameValueCollection> whiteListOfBlackKeyWords;

        /// <summary>
        /// 黑名單關鍵字過濾
        /// </summary>
        public BlackKeyWordFilter()
            : base()
        {
        }

        /// <summary>
        /// 指定黑名單
        /// </summary>
        public void SetBlackKeyWords(string[] keyWords)
        {
            blackKeyWords = keyWords;
        }

        /// <summary>
        /// 指定黑名單關鍵字在特定頁面中要允許的白名單(頁面名->關鍵字->,變數名,變數名,)
        /// </summary>
        public void SetWhiteListOfBlackKeyWords(Dictionary<string, NameValueCollection> whiteListOfBlackKeyWords)
        {
            this.whiteListOfBlackKeyWords = whiteListOfBlackKeyWords;
        }

        public override bool HandleRequest(ParamInfo paramInfo)
        {
            ShowDebugInfo(paramInfo);

            if (paramInfo.Value == null)
                return true;

            //拿掉空白後再檢查關鍵字
            string noSpaceValue = paramInfo.Value.Replace(" ", "");

            if (blackKeyWords != null)
            {
                foreach (string blackKeyWord in blackKeyWords)
                {
                    //檢查白名單
                    if (whiteListOfBlackKeyWords != null && whiteListOfBlackKeyWords.Keys.Any(x => string.Compare(x, paramInfo.ExecFilePath, true) == 0))
                    {
                        //取得以逗號相接的變數名單
                        string whiteKeyList = whiteListOfBlackKeyWords[paramInfo.ExecFilePath.ToLower()][blackKeyWord];

                        //在白名單內的跳過
                        if (whiteKeyList != null && whiteKeyList.IndexOf("," + paramInfo.Key + ",", StringComparison.CurrentCultureIgnoreCase) != -1)
                            continue;
                    }

                    if (noSpaceValue.IndexOf(blackKeyWord, StringComparison.CurrentCultureIgnoreCase) != -1)
                        return false;
                }
            }

            //沒有黑名單關鍵字也沒下一個參數過濾物件時,回報有效
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
        /// 用UrlDecode解碼參數內容
        /// </summary>
        public UrlDecodeParamValue()
            : base()
        {
        }

        public override bool HandleRequest(ParamInfo paramInfo)
        {
            ShowDebugInfo(paramInfo);

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
                successor = new BlackKeyWordFilter();

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
            ShowDebugInfo(paramInfo);

            // Acunetix 把 ctl00$contentplaceholder1$uclogin$txtAccount 改用 ctl00%24contentplaceholder1%24uclogin%24txtAccount 送來
            if (paramInfo.Key.IndexOf("%24") != -1)
                return false;

            if (successor == null)
                return true;

            //換下一個參數過濾物件檢查
            return successor.HandleRequest(paramInfo);
        }
    }
}
