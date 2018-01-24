<%@ Application Language="C#" %>
<%@ Import Namespace="Common.LogicObject" %>
<%@ Import Namespace="Common.DataAccess" %>
<%@ Import Namespace="Common.DataAccess.EmployeeAuthority" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e) 
    {
        // 在應用程式啟動時執行的程式碼

        //載入log4net設定
        log4net.Config.XmlConfigurator.Configure();

        //不使用Forms驗證時的設定
        //使用Forms驗證: 設定false時,「帳號」和「身分識別」使用Session值
        BackendPageCommon.UseFormsAuthentication = false;

        // 讓 ToSafeStr() 可依照前台、後台去調整過濾方式 
        GetSafeStringExtensions.IsBackendPage = true;

        string pswSalt1 = ConfigurationManager.AppSettings["PswSalt1"];
        string pswSalt2 = ConfigurationManager.AppSettings["PswSalt2"];
        Common.Utility.HashUtility.ChangePswSalt(pswSalt1, pswSalt2);
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

    protected void Application_AuthenticateRequest(object sender, EventArgs e)
    {
        //建立已登入的使用者資料
        //只在使用Forms驗證的狀況下
        if (BackendPageCommon.UseFormsAuthentication)
        {
            if (!Request.IsAuthenticated)
                return;

            //取得已登入的帳號
            string empAccount = User.Identity.Name;

            //取得員工身分
            EmployeeAuthorityLogic empAuth = new EmployeeAuthorityLogic(null);
            string roleName = empAuth.GetRoleNameOfEmp(empAccount);

            string[] aryRoles = new string[] { roleName };

            //建立使用者識別物件
            System.Security.Principal.GenericIdentity objIdentity = new System.Security.Principal.GenericIdentity(empAccount);

            //建立目前HTTP要求的使用者資料
            Context.User = new System.Security.Principal.GenericPrincipal(objIdentity, aryRoles);
        }
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
    }

    protected void Application_PostAcquireRequestState(object sender, EventArgs e)
    {
        log4net.ILog logger = log4net.LogManager.GetLogger(this.GetType());

        if (Context.Session == null)
        {
            logger.Info("Context.Session is null. skip this time.");
            return;
        }

        try
        {
            BackendPageCommon c = new BackendPageCommon(Context, null);
            int langNo = 1;

            if (Context.Session["seLangNoOfBackend"] == null)
            {
                //登入前,用 qsLangNo 處理後的值來重設語系
                langNo = c.qsLangNo;
            }
            else
            {
                //登入後,用 Session 值
                langNo = c.seLangNoOfBackend;
            }
            
            string cultureName = new LangManager().GetCultureName(langNo.ToString());
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(cultureName);
        }
        catch (Exception ex)
        {
            logger.Error("", ex);
        }

    }
    
</script>
