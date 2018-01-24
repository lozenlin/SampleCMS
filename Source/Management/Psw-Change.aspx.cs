using Common.LogicObject;
using Common.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Psw_Change : System.Web.UI.Page
{
    protected LoginCommonOfBackend c;
    protected EmployeeAuthorityLogic empAuth;

    private string ACCOUNT_FAILED_ERRMSG = "";

    /// <summary>
    /// 使用嚴格的密碼規則
    /// </summary>
    private bool useStrictPasswordRule = false;

    protected void Page_PreInit(object sender, EventArgs e)
    {
        c = new LoginCommonOfBackend(this.Context, this.ViewState);
        c.InitialLoggerOfUI(this.GetType());
        empAuth = new EmployeeAuthorityLogic();

        ACCOUNT_FAILED_ERRMSG = Resources.Lang.ErrMsg_AccountFailed;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadUIData();
        }

        Title = Resources.Lang.PswChange_Subtitle + " - " + Title;
    }

    private void LoadUIData()
    {
        txtAccount.Focus();

        revNewPsw.ErrorMessage = "*" + Resources.Lang.ErrMsg_PswNotQualified;
        cuvNewPsw.ErrorMessage = "*" + Resources.Lang.ErrMsg_PswNotQualified;
        covNewPswConfirm.ErrorMessage = "*" + Resources.Lang.ErrMsg_PswConfirmNotMatch;
        btnSubmit.ToolTip = Resources.Lang.Login_btnSubmit;
        btnBackToLogin.Title = Resources.Lang.Login_btnBackToLogin;
        cuvCheckCode.ErrorMessage = "*" + Resources.Lang.ErrMsg_WrongCheckCode;
        btnRefreshCodePic.Title = Resources.Lang.CaptchaPic_Hint;
        btnBackToLogin.HRef = StringUtility.SetParaValueInUrl(FormsAuthentication.LoginUrl, "l", c.qsLangNo.ToString());

        InitPasswordRule();
    }

    private void InitPasswordRule()
    {
        if (useStrictPasswordRule)
        {
            cuvNewPsw.Enabled = true;
            NewPswRuleNotice.InnerHtml = Resources.Lang.PswRuleNotice_StrictRule;
        }
        else
        {
            revNewPsw.Enabled = true;
            revNewPsw.ValidationExpression = StringUtility.GetPswSimpleRuleValidationExpression();
            NewPswRuleNotice.InnerHtml = Resources.Lang.PswRuleNotice_LooseRule;
        }
    }

    protected void cuvCheckCode_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = false;

        if (c.seCaptchaCode == "")
            args.IsValid = false;
        else if (txtCheckCode.Text.ToLower() == c.seCaptchaCode.ToLower())
            args.IsValid = true;
    }

    protected void cuvNewPsw_ServerValidate(object source, ServerValidateEventArgs args)
    {
        bool isValidPsw = true;

        //內文至少包含下列種類

        //特殊符號
        if (!Regex.IsMatch(args.Value, @"[~`!@#$%^&*()\-_=+,\.<>;':""\[\]{}\\|?/]{1,1}"))
        {
            isValidPsw = false;
        }

        //英文字母大寫
        if (!Regex.IsMatch(args.Value, @"[A-Z]+"))
        {
            isValidPsw = false;
        }

        //及小寫
        if (!Regex.IsMatch(args.Value, @"[a-z]+"))
        {
            isValidPsw = false;
        }

        //數字
        if (!Regex.IsMatch(args.Value, @"[0-9]+"))
        {
            isValidPsw = false;
        }

        //最少12字
        if (args.Value.Length < 12)
        {
            isValidPsw = false;
        }

        args.IsValid = isValidPsw;
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
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
            Master.ShowErrorMsg(string.Format("{0}: {1}", Resources.Lang.ErrMsg_Exception, empAuth.GetDbErrMsg()));
            //新增後端操作記錄
            empAuth.InsertBackEndLogData(new BackEndLogData()
            {
                EmpAccount = "",
                Description = string.Format("．變更密碼驗證時發生異常錯誤，帳號[{0}]　．An exception error occurred during change password verification! Account[{0}]", txtAccount.Text),
                IP = c.GetClientIP()
            });
            return;
        }

        //判斷是否有資料
        if (dsEmpVerify.Tables[0].Rows.Count == 0)
        {
            //沒資料
            Master.ShowErrorMsg(ACCOUNT_FAILED_ERRMSG);
            //新增後端操作記錄
            empAuth.InsertBackEndLogData(new BackEndLogData()
            {
                EmpAccount = "",
                Description = string.Format("．(變更密碼)帳號不存在，輸入帳號[{0}]　．(change password)Account doesn't exist! Account[{0}]", txtAccount.Text),
                IP = c.GetClientIP()
            });
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
            Master.ShowErrorMsg(ACCOUNT_FAILED_ERRMSG);
            //新增後端操作記錄
            empAuth.InsertBackEndLogData(new BackEndLogData()
            {
                EmpAccount = "",
                Description = string.Format("．(變更密碼)密碼錯誤，帳號[{0}]　．(change password)Password is incorrect! Account[{0}]", txtAccount.Text),
                IP = c.GetClientIP()
            });
            return;
        }

        //檢查是否停權
        if (Convert.ToBoolean(drEmpVerify["IsAccessDenied"]))
        {
            Master.ShowErrorMsg(Resources.Lang.ErrMsg_AccountUnavailable);
            //新增後端操作記錄
            empAuth.InsertBackEndLogData(new BackEndLogData()
            {
                EmpAccount = "",
                Description = string.Format("．(變更密碼)帳號停用，帳號[{0}]　．(change password)Account is denied! Account[{0}]", txtAccount.Text),
                IP = c.GetClientIP()
            });
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
                Master.ShowErrorMsg(Resources.Lang.ErrMsg_AccountUnavailable);
                //新增後端操作記錄
                empAuth.InsertBackEndLogData(new BackEndLogData()
                {
                    EmpAccount = "",
                    Description = string.Format("．(變更密碼)帳號超出有效範圍，帳號[{0}]　．(change password)Account validation date is out of range! Account[{0}]", txtAccount.Text),
                    IP = c.GetClientIP()
                });
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
            Master.ShowErrorMsg(string.Format("{0}: {1}", Resources.Lang.ErrMsg_Exception, empAuth.GetDbErrMsg()));
            //新增後端操作記錄
            empAuth.InsertBackEndLogData(new BackEndLogData()
            {
                EmpAccount = "",
                Description = string.Format("．(變更密碼)帳號登入取得使用者資料時發生異常錯誤，帳號[{0}]　．(change password)An exception error occurred during obtaining user profile! Account[{0}]", txtAccount.Text),
                IP = c.GetClientIP()
            });
            return;
        }

        DataRow drEmp = dsEmp.Tables[0].Rows[0];

    }

}