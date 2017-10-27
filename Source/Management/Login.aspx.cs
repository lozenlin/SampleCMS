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

    protected void Page_PreInit(object sender, EventArgs e)
    {
        c = new LoginCommonOfBackend(this.Context, this.ViewState);
        c.InitialLoggerOfUI(this.GetType());
        empAuth = new EmployeeAuthorityLogic(c);

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadUIData();
        }
    }

    private void LoadUIData()
    {
        ltrClientIP.Text = c.GetClientIP();
        CheckLoginFailedCountToShowCaptcha(false);
        txtAccount.Focus();
    }

    protected void btnLogin_Click(object sender, EventArgs e)
    {
        if (!IsValid)
            return;

        txtAccount.Text = txtAccount.Text.Trim();
        txtPassword.Text = txtPassword.Text.Trim();

        //登入驗證
        DataSet dsEmpVerify = empAuth.GetEmpDataToLogin(txtAccount.Text);

        if (dsEmpVerify == null)
        {
            //異常錯誤
            ShowErrorMsg(string.Format("{0}: {1}", "異常錯誤", empAuth.GetDbErrMsg()));
            //檢查登入失敗次數,是否顯示驗證圖
            CheckLoginFailedCountToShowCaptcha(true);
            return;
        }

        //判斷是否有資料
        if (dsEmpVerify.Tables[0].Rows.Count == 0)
        {
            //沒資料
            ShowErrorMsg("帳號或密碼錯誤");
            //檢查登入失敗次數,是否顯示驗證圖
            CheckLoginFailedCountToShowCaptcha(true);
            return;
        }

        //有資料
        DataRow drEmpVerify = dsEmpVerify.Tables[0].Rows[0];

        //檢查密碼
        string passwordHash = HashUtility.GetPasswordHash(txtPassword.Text);
        string empPassword = drEmpVerify["EmpPassword"].ToString();
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
            ShowErrorMsg("帳號或密碼錯誤");
            //檢查登入失敗次數,是否顯示驗證圖
            CheckLoginFailedCountToShowCaptcha(true);
            return;
        }

        //檢查是否停權
        if (Convert.ToBoolean(drEmpVerify["IsAccessDenied"]))
        {
            ShowErrorMsg("此帳號已被停權");
            //檢查登入失敗次數,是否顯示驗證圖
            CheckLoginFailedCountToShowCaptcha(true);
            return;
        }

        //檢查上架日期
        if (string.Compare(txtAccount.Text, "admin", true) != 0)    // 不檢查admin
        {
            DateTime startDate = Convert.ToDateTime(drEmpVerify["StartDate"]).Date;
            DateTime endDate = Convert.ToDateTime(drEmpVerify["EndDate"]).Date;
            DateTime today = DateTime.Today;

            if (today < startDate || endDate < today)
            {
                ShowErrorMsg("此帳號已被停權");
                //檢查登入失敗次數,是否顯示驗證圖
                CheckLoginFailedCountToShowCaptcha(true);
                return;
            }
        }

        //確認可登入後,取得使用者資料
        DataSet dsEmp = empAuth.GetEmpData(txtAccount.Text);

        if (dsEmp == null)
        {
            //異常錯誤
            ShowErrorMsg(string.Format("{0}: {1}", "異常錯誤", empAuth.GetDbErrMsg()));
            //檢查登入失敗次數,是否顯示驗證圖
            CheckLoginFailedCountToShowCaptcha(true);
            return;
        }

        //清除登入失敗次數
        c.seLoginFailedCount = 0;

        DataRow drEmp = dsEmp.Tables[0].Rows[0];
        LoginEmployeeData loginEmpData = new LoginEmployeeData()
        {
            EmpId = Convert.ToInt32(drEmp["EmpId"]),
            EmpName = drEmp["EmpName"].ToString(),
            Email = drEmp["Email"].ToString(),
            DeptId = Convert.ToInt32(drEmp["DeptId"]),
            DeptName = drEmp["DeptName"].ToString(),
            RoleId = Convert.ToInt32(drEmp["RoleId"]),
            RoleName = drEmp["RoleName"].ToString(),
            RoleDisplayName = drEmp["RoleDisplayName"].ToString(),
            StartDate = Convert.ToDateTime(drEmp["StartDate"]),
            EndDate = Convert.ToDateTime(drEmp["EndDate"]),
            EmpAccount = drEmp["EmpAccount"].ToString()
        };
        c.SaveLoginEmployeeDataIntoSession(loginEmpData);

        //後端操作記錄

        //設定已登入
        FormsAuthentication.RedirectFromLoginPage(c.seLoginEmpData.EmpAccount, false);

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