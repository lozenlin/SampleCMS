<%@ Application Language="C#" %>
<%@ Import Namespace="Common.LogicObject" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e) 
    {
        // 在應用程式啟動時執行的程式碼

        //載入log4net設定
        log4net.Config.XmlConfigurator.Configure();

    }
    
    void Application_End(object sender, EventArgs e) 
    {
        //  在應用程式關閉時執行的程式碼

    }
        
    void Application_Error(object sender, EventArgs e) 
    { 
        // 在發生未處理的錯誤時執行的程式碼

    }

    void Session_Start(object sender, EventArgs e) 
    {
        // 在新的工作階段啟動時執行的程式碼

    }

    void Session_End(object sender, EventArgs e) 
    {
        // 在工作階段結束時執行的程式碼
        // 注意: 只有在  Web.config 檔案中將 sessionstate 模式設定為 InProc 時，
        // 才會引起 Session_End 事件。如果將 session 模式設定為 StateServer 
        // 或 SQLServer，則不會引起該事件。

    }

    protected void Application_BeginRequest(object sender, EventArgs e)
    {
        bool isForceUseSSL = false;
        
        if (bool.TryParse(ConfigurationManager.AppSettings["IsForceUseSSL"] ?? "", out isForceUseSSL))
        {
        }

        if (isForceUseSSL && string.Compare(Context.Request.Url.Scheme, "https", true) != 0)
        {
            string newUrl = "https://" + Context.Request.Url.Host + Context.Request.Url.PathAndQuery;
            Response.Redirect(newUrl);
        }

        log4net.ILog logger = log4net.LogManager.GetLogger(this.GetType());

        ////計算檢查時間
        //System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
        //sw.Start();

        //檢查參數內容是否有效
        ParamFilterClient paramFilterClient = new ParamFilterClient();

        if (!paramFilterClient.IsParamValueValid(Context))
        {
            ////顯示檢查時間
            //sw.Stop();
            //System.Diagnostics.Debug.WriteLine(sw.Elapsed.TotalMilliseconds.ToString() + "ms");

            ////產生404錯誤
            //throw new HttpException(404, "Invalid Parameter!");

            Response.Redirect("~/ErrorPage.aspx#InvalidParameter");
        }

        ////顯示檢查時間
        //sw.Stop();
        //System.Diagnostics.Debug.WriteLine(sw.Elapsed.TotalMilliseconds.ToString() + "ms");

        try
        {
            PageCommon c = new PageCommon(Context, null);

            //一律用 qsLangNo 處理後的值來重設語系
            string cultureName = new LangManager().GetCultureName(c.qsLangNo.ToString());
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(cultureName);
        }
        catch (Exception ex)
        {
            logger.Error("", ex);
        }
    }
</script>
