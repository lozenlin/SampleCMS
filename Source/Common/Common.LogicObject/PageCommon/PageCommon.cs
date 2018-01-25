// ===============================================================================
// PageCommon of SampleCMS
// https://github.com/lozenlin/SampleCMS
//
// PageCommon.cs
//
// ===============================================================================
// Copyright (c) 2017 lozenlin
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// ===============================================================================

using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;

namespace Common.LogicObject
{
    /// <summary>
    /// 網頁的共用元件
    /// </summary>
    public class PageCommon
    {
        /// <summary>
        /// UI 程式(Page, UserControl, HttpHandler)的記錄工具
        /// </summary>
        public ILog LoggerOfUI
        {
            get;
            private set;
        }

        #region qs:=QueryString, se:=Session, vs:=ViewState, co:=Cookie

        /// <summary>
        /// 語言號碼(l或lang,l優先)
        /// </summary>
        public virtual int qsLangNo
        {
            get
            {
                string str = QueryStringToSafeStr("l");
                if (str == null)
                    str = QueryStringToSafeStr("lang");

                int nResult;

                if (str != null)
                    str = str.Trim();

                if (string.IsNullOrEmpty(str))
                {
                    //未指定,抓瀏覽器的
                    string resultCultureName = GetAllowedUserCultureName();

                    nResult = Convert.ToInt32(new LangManager().GetLangNo(resultCultureName));
                }
                else if (int.TryParse(str, out nResult))
                {
                }

                return nResult;
            }
        }

        public string qsCultureNameOfLangNo
        {
            get
            {
                string result = new LangManager().GetCultureName(qsLangNo.ToString());
                return result;
            }
        }

        /// <summary>
        /// 頁碼
        /// </summary>
        public int qsPageCode
        {
            get
            {
                int nResult;
                string str = QueryStringToSafeStr("p");

                if (str != null && int.TryParse(str, out nResult) && nResult >= 1)
                {
                }
                else
                    nResult = 1;

                return nResult;
            }
        }

        /// <summary>
        /// 代碼
        /// </summary>
        public int qsId
        {
            get
            {
                string str = QueryStringToSafeStr("id");
                int nResult = 0;

                if (str != null && int.TryParse(str, out nResult))
                {
                }
                else
                    nResult = 0;

                return nResult;
            }
        }

        /// <summary>
        /// Attach file id
        /// </summary>
        public Guid qsAttId
        {
            get
            {
                string str = QueryStringToSafeStr("attid") ?? "";
                Guid result = Guid.Empty;

                if (str != "" && Guid.TryParse(str, out result))
                {
                }

                return result;
            }
        }

        /// <summary>
        /// Picture id
        /// </summary>
        public Guid qsPicId
        {
            get
            {
                string str = QueryStringToSafeStr("picid") ?? "";
                Guid result = Guid.Empty;

                if (str != "" && Guid.TryParse(str, out result))
                {
                }

                return result;
            }
        }

        /// <summary>
        /// Video id
        /// </summary>
        public Guid qsVidId
        {
            get
            {
                string str = QueryStringToSafeStr("vidid") ?? "";
                Guid result = Guid.Empty;

                if (str != "" && Guid.TryParse(str, out result))
                {
                }

                return result;
            }
        }

        /// <summary>
        /// Token value to preview article page
        /// </summary>
        public string qsPreview
        {
            get
            {
                return QueryStringToSafeStr("preview");
            }
        }

        #endregion

        protected HttpContext context;
        protected StateBag viewState;
        protected ILog logger = null;
        /// <summary>
        /// 讓 QueryStringToSafeStr() 可依照前台、後台去調整過濾方式
        /// </summary>
        protected bool isBackendPage = false;

        #region 工具屬性

        protected HttpServerUtility Server
        {
            get { return context.Server; }
        }

        protected HttpRequest Request
        {
            get { return context.Request; }
        }

        protected HttpResponse Response
        {
            get { return context.Response; }
        }

        protected HttpSessionState Session
        {
            get { return context.Session; }
        }

        protected IPrincipal User
        {
            get { return context.User; }
        }

        protected StateBag ViewState
        {
            get { return viewState; }
        }

        protected System.Web.Caching.Cache Cache
        {
            get { return context.Cache; }
        }

        #endregion

        /// <summary>
        /// 網頁的共用元件
        /// </summary>
        public PageCommon(HttpContext context, StateBag viewState)
        {
            this.context = context;
            this.viewState = viewState;
            logger = LogManager.GetLogger(this.GetType());
            isBackendPage = (this is BackendPageCommon);
        }

        /// <summary>
        /// 初始化 LoggerOfUI
        /// </summary>
        public ILog InitialLoggerOfUI(Type typeUiComponent)
        {
            LoggerOfUI = LogManager.GetLogger(typeUiComponent);

            return LoggerOfUI;
        }

        public string QueryStringToSafeStr(string name)
        {
            string value = Request.QueryString[name];

            if (value == null)
                return null;

            //if (!isBackendPage)
            //return AntiXss.GetSafeHtmlFragment(value);
            return value;
        }

        public string FormToSafeStr(string name)
        {
            string value = Request.Form[name];

            if (value == null)
                return null;

            //if (!isBackendPage)
            //return AntiXss.GetSafeHtmlFragment(value);
            return value;
        }

        public string SessionToSafeStr(string name)
        {
            object obj = Session[name];

            if (obj == null)
                return null;

            string value = Convert.ToString(obj);

            //if (!isBackendPage)
            //return AntiXss.GetSafeHtmlFragment(value);
            return value;
        }

        /// <summary>
        /// 取得客戶端主機位址
        /// </summary>
        /// <remarks>
        /// reference:http://www.dotblogs.com.tw/hunterpo/archive/2011/03/21/21991.aspx
        /// </remarks>
        public string GetClientIP()
        {
            string result = Request.ServerVariables["REMOTE_ADDR"] ?? "";

            if (null != Request.ServerVariables["HTTP_VIA"])
            {
                string forwardIP = Request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? "";

                if (forwardIP != "" && forwardIP.ToLower() != "unknown")
                    result = forwardIP;
            }

            return result;
        }

        /// <summary>
        /// 取得客戶端主機 IPv4 位址
        /// </summary>
        /// <remarks>
        /// reference:http://www.dotblogs.com.tw/hunterpo/archive/2011/03/21/21991.aspx
        /// </remarks>
        public string GetClientIPv4()
        {
            string result = "";

            foreach (IPAddress ip in Dns.GetHostAddresses(GetClientIP()))
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    result = ip.ToString();
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// 取得客戶端主機 IPv6 位址
        /// </summary>
        /// <remarks>
        /// reference:http://www.dotblogs.com.tw/hunterpo/archive/2011/03/21/21991.aspx
        /// </remarks>
        public string GetClientIPv6()
        {
            string result = "";

            foreach (IPAddress ip in Dns.GetHostAddresses(GetClientIP()))
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                {
                    result = ip.ToString();
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// 取得本系統允許的瀏覽器指定語系名稱
        /// </summary>
        public string GetAllowedUserCultureName()
        {
            string resultCultureName = "";

            if (Request.UserLanguages != null)
            {
                foreach (string userLang in Request.UserLanguages)
                {
                    string tempCultureName = userLang;

                    if (tempCultureName.Contains(";"))
                        tempCultureName = tempCultureName.Split(';')[0];

                    if (tempCultureName.StartsWith("zh-") || tempCultureName == "zh")
                    {
                        resultCultureName = LangManager.CultureNameZHTW;
                        break;
                    }
                    else if (tempCultureName.StartsWith("en-") || tempCultureName == "en")
                    {
                        resultCultureName = LangManager.CultureNameEN;
                        break;
                    }
                }
            }

            if (resultCultureName == "")
                resultCultureName = LangManager.CultureNameZHTW;

            return resultCultureName;
        }

        /// <summary>
        /// Finds a Control recursively. Note finds the first match and exists
        /// </summary>
        /// <remarks>
        /// reference: http://www.west-wind.com/weblog/posts/2006/Apr/09/ASPNET-20-MasterPages-and-FindControl
        /// </remarks>
        public Control FindControlRecursive(Control Root, string Id)
        {
            if (Root.ID == Id)
                return Root;

            foreach (Control Ctl in Root.Controls)
            {
                Control FoundCtl = FindControlRecursive(Ctl, Id);
                if (FoundCtl != null)
                    return FoundCtl;
            }

            return null;
        }

        /// <summary>
        /// 在傳入的連結後面附加目前的網址參數
        /// </summary>
        public string AppendCurrentQueryString(string linkUrl)
        {
            StringBuilder sbResult = new StringBuilder(linkUrl);
            bool hasPara = linkUrl.IndexOf("?") != -1;
            bool isFirstPara = true;

            foreach (string key in Request.QueryString.AllKeys)
            {
                // add separator
                if (isFirstPara && !hasPara)
                {
                    sbResult.Append("?");
                    isFirstPara = false;
                }
                else
                {
                    sbResult.Append("&");
                }

                sbResult.AppendFormat("{0}={1}", key, Server.UrlEncode(Request.QueryString[key]));
            }

            return sbResult.ToString();
        }

        /// <summary>
        /// 只寄給測試收件人
        /// </summary>
        public bool IsSendToTesterOnly()
        {
            bool bResult = false;
            bool.TryParse(ConfigurationManager.AppSettings["SendToTesterOnly"], out bResult);

            return bResult;
        }

        /// <summary>
        /// display ServiceEmail but send mail by sender
        /// </summary>
        public bool UseSender()
        {
            bool bResult = false;
            bool.TryParse(ConfigurationManager.AppSettings["UseSender"], out bResult);

            return bResult;
        }
    }
}
