using Common.LogicObject;
using log4net;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

/// <summary>
/// 參數過濾設定與執行
/// </summary>
public class ParamFilterClient
{
    protected ILog logger;

    /// <summary>
    /// Int32 類型的參數名單 (needs lowercase)
    /// </summary>
    private List<string> intParams = new List<string>(new string[] { 
        "l", "lang", "p", "id", "ckeditorfuncnum",
        "thumb", "emprange", "deptid", "empid", "roleid",
        "rangemode", "w", "h", "saveas", "stretch", 
        "flexable"
    });

    /// <summary>
    /// 日期類型的參數名單 (needs lowercase)
    /// </summary>
    private List<string> dateTimeParams = new List<string>(new string[] { 
        "startdate", "enddate"
    });

    /// <summary>
    /// Guid 類型的參數名單 (needs lowercase)
    /// </summary>
    private List<string> guidParams = new List<string>(new string[] { 
        "artid", "attid", "picid", "vidid"
    });

    /// <summary>
    /// 黑名單 (needs lowercase)
    /// </summary>
    private string[] blacklistKeywords = new string[] {
        "javascript:", "vbscript:", "mocha:", 
        "livescript:", "<script", "alert(", 
        "../../etc/passwd", "../../windows/win.ini", "xp_cmdshell", 
        "acustart", "acuend", "prompt(", 
        "<metahttp-equiv", "waitfordelay", "sleep(", 
        "window.location", "dow.loca", "substring",
        "db_name", "sysprocesses", "db_", 
        "${", "#{", "t(", 
        "msgbox(", "'():;", "onmouse", 
        "onresize", "ssion("
    };

    /// <summary>
    /// Regex黑名單
    /// </summary>
    private string[] blacklistPatterns = new string[] { 
        "<[a-zA-Z0-9]+ [a-zA-Z0-9'\"]+=[a-zA-Z0-9'\"]+>", // e.g., <% contenteditable onresize=HVUx(9663)> <%div style=width:expression(S0K4(9408))>
        "[a-zA-Z0-9]+<[a-zA-Z0-9]+<"    // e.g., yfwribon<8FoJTt<  e<7k8rzz<
    };

    /// <summary>
    /// 有限制長度的字串參數名稱與內容長度對照表
    /// </summary>
    private Dictionary<string, int> paramValueLenLookup = new Dictionary<string, int>();

    /// <summary>
    /// 黑名單關鍵字在特定頁面中要允許的白名單
    /// ( 頁面名[需小寫] -> 關鍵字 -> ,變數名,變數名, )
    /// ( execFilePath[needs lowercase] -> Keyword -> ,ParamName,ParamName, )
    /// </summary>
    private Dictionary<string, NameValueCollection> whitelistOfBlacklistKeywords = new Dictionary<string, NameValueCollection>();

    public ParamFilterClient()
    {
        logger = LogManager.GetLogger(this.GetType());
        InitialParamValueLenLookup();
        InitialWhitelistOfBlacklistKeywords();
    }

    private void InitialParamValueLenLookup()
    {
        // (needs lowercase)
        paramValueLenLookup.Add("kw", 100);
        paramValueLenLookup.Add("pkw", 100);
        paramValueLenLookup.Add("desckw", 100);
        paramValueLenLookup.Add("preview", 256);
        paramValueLenLookup.Add("servicename", 50);
        paramValueLenLookup.Add("term", 100);
        paramValueLenLookup.Add("listtype", 20);
        paramValueLenLookup.Add("fnselected", 50);
        paramValueLenLookup.Add("ckeditor", 50);
        paramValueLenLookup.Add("path", 2048);
        paramValueLenLookup.Add("sortfield", 50);
        paramValueLenLookup.Add("issortdesc", 5);
        paramValueLenLookup.Add("act", 10);
        paramValueLenLookup.Add("pparents", 50);
        paramValueLenLookup.Add("account", 20);
        paramValueLenLookup.Add("ip", 20);
        paramValueLenLookup.Add("isacckw", 5);
        paramValueLenLookup.Add("isipheadkw", 5);
        paramValueLenLookup.Add("ctltext", 50);
        paramValueLenLookup.Add("url", 2048);
        paramValueLenLookup.Add("token", 256);
        paramValueLenLookup.Add("location", 2048);
        paramValueLenLookup.Add("returnurl", 500);
    }

    private void InitialWhitelistOfBlacklistKeywords()
    {
        StringBuilder sbParas = null;
        NameValueCollection keyWordAndParamLookup = new NameValueCollection();

        //Article-Config.aspx
        sbParas = new StringBuilder();

        // ,變數名,變數名, (,ParamName,ParamName,)
        sbParas.Append(",")
            .Append("ctl00$cphContent$txtCkeContextZhTw").Append(",")
            .Append("ctl00$cphContent$txtCkeContextEn").Append(",");

        keyWordAndParamLookup = new NameValueCollection();

        // 關鍵字 (Keyword)
        // needs lowercase
        keyWordAndParamLookup.Add("<script", sbParas.ToString());

        // 頁面名 (execFilePath)
        // needs lowercase
        whitelistOfBlacklistKeywords.Add("article-config.aspx", keyWordAndParamLookup);
    }

    /// <summary>
    /// 參數內容是否有效
    /// </summary>
    public bool IsParamValueValid(HttpContext context)
    {
        if (context == null)
            return true;

        //取得要求執行的網頁檔名
        string execFilePath = context.Request.AppRelativeCurrentExecutionFilePath.Replace("~/", "");

        //檔名後面不准接斜線
        if (execFilePath != "" && context.Request.RawUrl.Contains(execFilePath + "/"))
            return false;

        //網址參數內容是否有效
        if (!IsQueryStringValueValid(execFilePath, context.Request.QueryString))
            return false;

        //POST參數內容是否有效
        if (!IsPostValueValid(execFilePath, context.Request.Form))
            return false;

        return true;
    }

    /// <summary>
    /// 網址參數內容是否有效
    /// </summary>
    public bool IsQueryStringValueValid(string execFilePath, NameValueCollection queryString)
    {
        if (queryString == null || queryString.Count == 0)
            return true;

        //建立參數過濾物件
        //特殊頁面參數過濾
        SpecificPageParamFilter specificPageParam = new SpecificPageParamFilter();

        //非字串參數過濾
        NonStringParamFilter nonStringParam = new NonStringParamFilter();
        // 指定參數名單
        nonStringParam.SetIntParamList(intParams);
        nonStringParam.SetDateTimeParamList(dateTimeParams);
        nonStringParam.SetGuidParamList(guidParams);

        //有限制長度的字串參數過濾
        LimitedStringParamFilter limitedStringParam = new LimitedStringParamFilter();
        // 指定參數名稱與內容長度對照表
        limitedStringParam.SetParamValueLenLookup(paramValueLenLookup);

        //規則表達式黑名單過濾
        RegexParamFilter regexParam = new RegexParamFilter();
        regexParam.SetBlacklistPatterns(blacklistPatterns);

        //SQL Injection 過濾
        SQLInjectionFilterExt sqlInjection1 = new SQLInjectionFilterExt();

        //用 HtmlDecode 解碼參數內容
        HtmlDecodeParamValue htmlDecodeValue = new HtmlDecodeParamValue();

        //黑名單關鍵字過濾
        BlacklistKeywordFilter blacklistKw = new BlacklistKeywordFilter();
        // 指定黑名單
        blacklistKw.SetBlacklistKeywords(blacklistKeywords);

        //SQL Injection過濾
        SQLInjectionFilterExt sqlInjection2 = new SQLInjectionFilterExt();

        //建立檢查順序
        ParamFilter chainOfResponsibility = specificPageParam;
        specificPageParam.SetSuccessor(nonStringParam);
        nonStringParam.SetSuccessor(limitedStringParam);
        limitedStringParam.SetSuccessor(regexParam);
        regexParam.SetSuccessor(sqlInjection1);
        sqlInjection1.SetSuccessor(htmlDecodeValue);
        htmlDecodeValue.SetSuccessor(blacklistKw);
        blacklistKw.SetSuccessor(sqlInjection2);

        //開始檢查
        logger.Debug("checking queryString.Keys");

        foreach (string key in queryString.Keys)
        {
            if (key == null || queryString[key] == null || queryString[key].Length == 0)
            {
                logger.DebugFormat("skip key[{0}]", key);
                continue;
            }

            //檢查參數名稱
            if (Regex.IsMatch(key, "[\"']"))
            {
                logger.InfoFormat("key[{0}] Failed!", key);
                return false;
            }

            //參數內容是否有效
            ParamFilter.ParamInfo paramInfo = new ParamFilter.ParamInfo()
            {
                Key = key,
                Value = queryString[key],
                ExecFilePath = execFilePath
            };

            if (!chainOfResponsibility.HandleRequest(paramInfo))
                return false;
        }

        return true;
    }

    /// <summary>
    /// POST參數內容是否有效
    /// </summary>
    public bool IsPostValueValid(string execFilePath, NameValueCollection requestForm)
    {
        if (requestForm == null || requestForm.Count == 0)
            return true;

        bool useSimplifedChain = true;
        string lowerFileName = execFilePath.ToLower();

        if (lowerFileName == "login.aspx"
            || lowerFileName == "psw-change.aspx"
            || lowerFileName == "psw-require.aspx"
            || lowerFileName == "captcha.ashx"
            || lowerFileName == "errorpage.aspx"
            )
        {
            useSimplifedChain = false;
        }

        //規則表達式黑名單過濾
        RegexParamFilter regexParam = new RegexParamFilter();
        regexParam.SetBlacklistPatterns(blacklistPatterns);

        //黑名單關鍵字過濾
        BlacklistKeywordFilter blacklistKw = new BlacklistKeywordFilter();
        // 指定黑名單
        blacklistKw.SetBlacklistKeywords(blacklistKeywords);
        // 指定黑名單關鍵字在特定頁面中要允許的白名單
        blacklistKw.SetWhitelistOfBlacklistKeywords(whitelistOfBlacklistKeywords);

        //建立檢查順序
        ParamFilter chainOfResponsibility = null;

        if (useSimplifedChain)
        {
            //簡易版參數過濾規則
            chainOfResponsibility = regexParam;
            regexParam.SetSuccessor(blacklistKw);
        }
        else
        {
            //針對 Acunetix 送來的 Post 參數過濾
            ForAcunetixPostParamFilter forAcunetix = new ForAcunetixPostParamFilter();

            //特殊頁面參數過濾
            SpecificPageParamFilter specificPageParam = new SpecificPageParamFilter();

            //用 HtmlDecode 解碼參數內容
            HtmlDecodeParamValue htmlDecodeValue = new HtmlDecodeParamValue();

            //SQL Injection過濾
            SQLInjectionFilterExt sqlInjection1 = new SQLInjectionFilterExt();

            //用 UrlDecode 解碼參數內容
            UrlDecodeParamValue urlDecodeValue = new UrlDecodeParamValue();

            //SQL Injection過濾
            SQLInjectionFilterExt sqlInjection2 = new SQLInjectionFilterExt();

            chainOfResponsibility = forAcunetix;
            forAcunetix.SetSuccessor(specificPageParam);
            specificPageParam.SetSuccessor(regexParam);
            regexParam.SetSuccessor(htmlDecodeValue);
            htmlDecodeValue.SetSuccessor(sqlInjection1);
            sqlInjection1.SetSuccessor(urlDecodeValue);
            urlDecodeValue.SetSuccessor(blacklistKw);
            blacklistKw.SetSuccessor(sqlInjection2);
        }

        //開始檢查
        logger.Debug("checking requestForm.Keys");

        foreach (string key in requestForm.Keys)
        {
            if (key == null || requestForm[key] == null || requestForm[key].Length == 0)
            {
                logger.DebugFormat("skip key[{0}]", key);
                continue;
            }

            if (key.StartsWith("__"))
            {
                logger.DebugFormat("skip key[{0}]", key);
                continue;
            }

            //參數內容是否有效
            ParamFilter.ParamInfo paramInfo = new ParamFilter.ParamInfo()
            {
                Key = key,
                Value = requestForm[key],
                ExecFilePath = execFilePath
            };

            if (!chainOfResponsibility.HandleRequest(paramInfo))
                return false;
        }

        return true;
    }

}