using Common.LogicObject;
using Common.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Login : System.Web.UI.Page
{
    protected LoginCommonOfBackend c;
    protected EmployeeAuthorityLogic empAuth;

    /// <summary>
    /// 達到需要驗證碼輸入的登入失敗次數
    /// </summary>
    private const int MAX_FAILED_COUNT_TO_SHOW_CAPTCHA = 2;

    private string ACCOUNT_FAILED_ERRMSG = "";

    protected void Page_PreInit(object sender, EventArgs e)
    {
        c = new LoginCommonOfBackend(this.Context, this.ViewState);
        c.InitialLoggerOfUI(this.GetType());
        empAuth = new EmployeeAuthorityLogic();

        ACCOUNT_FAILED_ERRMSG = Resources.Lang.ErrMsg_AccountFailed;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        ltrBackStageName.Text = Resources.Lang.BackStageName;

        if (!IsPostBack)
        {
            LoadUIData();
        }

        Title = ltrBackStageName.Text;
    }

    private void LoadUIData()
    {
        ltrClientIP.Text = c.GetClientIP();
        CheckLoginFailedCountToShowCaptcha(false);
        txtAccount.Focus();

        btnLogin.ToolTip = Resources.Lang.Login_btnLogin;
        btnChangePsw.Title = Resources.Lang.Login_btnChangePsw;
        btnChangePsw.HRef = string.Format("Psw-Change.aspx?l={0}", c.qsLangNo);
        btnDontRememberPsw.Title = Resources.Lang.Login_btnDontRememberPsw;
        btnDontRememberPsw.HRef = string.Format("Psw-Require.aspx?l={0}", c.qsLangNo);
        cuvCheckCode.ErrorMessage = "*" + Resources.Lang.ErrMsg_WrongCheckCode;
        btnRefreshCodePic.Title = Resources.Lang.CaptchaPic_Hint;

        btnChangLang.HRef = Request.AppRelativeCurrentExecutionFilePath + "?l=2";

        if (c.qsLangNo == 2)
        {
            btnChangLang.HRef = Request.AppRelativeCurrentExecutionFilePath + "?l=1";
            btnChangLang.InnerHtml = "中文";
        }
    }

    protected void btnLogin_Click(object sender, EventArgs e)
    {
        txtCheckCode.Text = "";

        if (!IsValid)
            return;

        txtAccount.Text = txtAccount.Text.Trim();
        txtPassword.Text = txtPassword.Text.Trim();

        //登入驗證
        DataSet dsEmpVerify = empAuth.GetEmployeeDataToLogin(txtAccount.Text);

        if (dsEmpVerify == null)
        {
            //異常錯誤
            ShowErrorMsg(string.Format("{0}: {1}", Resources.Lang.ErrMsg_Exception, empAuth.GetDbErrMsg()));
            //新增後端操作記錄
            empAuth.InsertBackEndLogData(new BackEndLogData()
            {
                EmpAccount = "",
                Description = string.Format("．帳號登入驗證時發生異常錯誤，帳號[{0}]　．An exception error occurred during login verification! Account[{0}]", txtAccount.Text),
                IP = c.GetClientIP()
            });
            //檢查登入失敗次數,是否顯示驗證圖
            CheckLoginFailedCountToShowCaptcha(true);
            return;
        }

        //判斷是否有資料
        if (dsEmpVerify.Tables[0].Rows.Count == 0)
        {
            //沒資料
            ShowErrorMsg(ACCOUNT_FAILED_ERRMSG);
            //新增後端操作記錄
            empAuth.InsertBackEndLogData(new BackEndLogData()
            {
                EmpAccount = "",
                Description = string.Format("．帳號不存在，輸入帳號[{0}]　．Account doesn't exist! Account[{0}]", txtAccount.Text),
                IP = c.GetClientIP()
            });
            //檢查登入失敗次數,是否顯示驗證圖
            CheckLoginFailedCountToShowCaptcha(true);
            return;
        }

        //有資料
        DataRow drEmpVerify = dsEmpVerify.Tables[0].Rows[0];

        //檢查密碼
        string passwordHash = HashUtility.GetPasswordHash(txtPassword.Text);
        string empPassword = drEmpVerify.ToSafeStr("EmpPassword");
        bool isPasswordCorrect = false;

        if (Convert.ToBoolean(drEmpVerify["PasswordHashed"]))
        {
            isPasswordCorrect = (passwordHash == empPassword);
        }
        else
        {
            isPasswordCorrect = (txtPassword.Text == empPassword);
        }

        if (!isPasswordCorrect)
        {
            ShowErrorMsg(ACCOUNT_FAILED_ERRMSG);
            //新增後端操作記錄
            empAuth.InsertBackEndLogData(new BackEndLogData()
            {
                EmpAccount = "",
                Description = string.Format("．密碼錯誤，帳號[{0}]　．Password is incorrect! Account[{0}]", txtAccount.Text),
                IP = c.GetClientIP()
            });
            //檢查登入失敗次數,是否顯示驗證圖
            CheckLoginFailedCountToShowCaptcha(true);
            return;
        }

        //檢查是否停權
        if (Convert.ToBoolean(drEmpVerify["IsAccessDenied"]))
        {
            ShowErrorMsg(Resources.Lang.ErrMsg_AccountUnavailable);
            //新增後端操作記錄
            empAuth.InsertBackEndLogData(new BackEndLogData()
            {
                EmpAccount = "",
                Description = string.Format("．帳號停用，帳號[{0}]　．Account is denied! Account[{0}]", txtAccount.Text),
                IP = c.GetClientIP()
            });
            //檢查登入失敗次數,是否顯示驗證圖
            CheckLoginFailedCountToShowCaptcha(true);
            return;
        }

        //檢查上架日期
        if (string.Compare(txtAccount.Text, "admin", true) != 0)    // 不檢查帳號 admin
        {
            DateTime startDate = Convert.ToDateTime(drEmpVerify["StartDate"]).Date;
            DateTime endDate = Convert.ToDateTime(drEmpVerify["EndDate"]).Date;
            DateTime today = DateTime.Today;

            if (today < startDate || endDate < today)
            {
                ShowErrorMsg(Resources.Lang.ErrMsg_AccountUnavailable);
                //新增後端操作記錄
                empAuth.InsertBackEndLogData(new BackEndLogData()
                {
                    EmpAccount = "",
                    Description = string.Format("．帳號超出有效範圍，帳號[{0}]　．Account validation date is out of range! Account[{0}]", txtAccount.Text),
                    IP = c.GetClientIP()
                });
                //檢查登入失敗次數,是否顯示驗證圖
                CheckLoginFailedCountToShowCaptcha(true);
                return;
            }
        }

        //記錄登入時間與IP
        empAuth.UpdateEmployeeLoginInfo(txtAccount.Text, c.GetClientIP());

        //確認可登入後,取得員工資料
        DataSet dsEmp = empAuth.GetEmployeeData(txtAccount.Text);

        if (dsEmp == null)
        {
            //異常錯誤
            ShowErrorMsg(string.Format("{0}: {1}", Resources.Lang.ErrMsg_Exception, empAuth.GetDbErrMsg()));
            //新增後端操作記錄
            empAuth.InsertBackEndLogData(new BackEndLogData()
            {
                EmpAccount = "",
                Description = string.Format("．帳號登入取得使用者資料時發生異常錯誤，帳號[{0}]　．An exception error occurred during obtaining user profile! Account[{0}]", txtAccount.Text),
                IP = c.GetClientIP()
            });
            //檢查登入失敗次數,是否顯示驗證圖
            CheckLoginFailedCountToShowCaptcha(true);
            return;
        }

        //清除登入失敗次數
        c.seLoginFailedCount = 0;

        DataRow drEmp = dsEmp.Tables[0].Rows[0];

        DateTime 
            thisLoginTime = DateTime.MinValue,
            lastLoginTime = DateTime.MinValue;

        if (!Convert.IsDBNull(drEmp["ThisLoginTime"]))
            thisLoginTime = Convert.ToDateTime(drEmp["ThisLoginTime"]);

        if (!Convert.IsDBNull(drEmp["LastLoginTime"]))
            lastLoginTime = Convert.ToDateTime(drEmp["LastLoginTime"]);

        LoginEmployeeData loginEmpData = new LoginEmployeeData()
        {
            EmpId = Convert.ToInt32(drEmp["EmpId"]),
            EmpName = drEmp.ToSafeStr("EmpName"),
            Email = drEmp.ToSafeStr("Email"),
            DeptId = Convert.ToInt32(drEmp["DeptId"]),
            DeptName = drEmp.ToSafeStr("DeptName"),
            RoleId = Convert.ToInt32(drEmp["RoleId"]),
            RoleName = drEmp.ToSafeStr("RoleName"),
            RoleDisplayName = drEmp.ToSafeStr("RoleDisplayName"),
            StartDate = Convert.ToDateTime(drEmp["StartDate"]),
            EndDate = Convert.ToDateTime(drEmp["EndDate"]),
            EmpAccount = drEmp.ToSafeStr("EmpAccount"),
            ThisLoginTime = thisLoginTime,
            ThisLoginIP = drEmp.ToSafeStr("ThisLoginIP"),
            LastLoginTime = lastLoginTime,
            LastLoginIP = drEmp.ToSafeStr("LastLoginIP")
        };
        c.SaveLoginEmployeeDataIntoSession(loginEmpData);

        //新增後端操作記錄
        empAuth.InsertBackEndLogData(new BackEndLogData()
        {
            EmpAccount = c.GetEmpAccount(),
            Description = "．登入系統！　．Logged in!",
            IP = c.GetClientIP()
        });

        //記錄指定語系
        c.seLangNoOfBackend = c.qsLangNo;

        //設定已登入
        FormsAuthentication.RedirectFromLoginPage(c.seLoginEmpData.EmpAccount, false);

        /* 需要帶入額外參數時使用
        if (string.IsNullOrEmpty(Request.QueryString["ReturnUrl"]))
        {
            FormsAuthentication.SetAuthCookie(c.seLoginEmpData.EmpAccount, false);
            Response.Redirect(FormsAuthentication.DefaultUrl + "?l=" + c.qsLangNo.ToString());
        }
         */
    }

    private void ShowErrorMsg(string value)
    {
        ltrErrMsg.Text = value;
        ErrorMsgArea.Visible = (value != "");
    }

    protected void cuvCheckCode_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = false;

        if (c.seCaptchaCode == "")
            args.IsValid = false;
        else if (txtCheckCode.Text.ToLower() == c.seCaptchaCode.ToLower())
            args.IsValid = true;
    }

    /// <summary>
    /// 檢查登入失敗次數,是否顯示驗證圖
    /// </summary>
    private void CheckLoginFailedCountToShowCaptcha(bool isIncreaseLoginFailedCount)
    {
        //記錄登入失敗次數
        if (isIncreaseLoginFailedCount)
            c.seLoginFailedCount += 1;

        if (c.seLoginFailedCount >= MAX_FAILED_COUNT_TO_SHOW_CAPTCHA)
            CheckCodeArea.Visible = true;
    }
}