using Common.LogicObject;
using Common.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
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
            HandleResetPswRequirement();
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

    /// <summary>
    /// 處理重置密碼要求
    /// </summary>
    private void HandleResetPswRequirement()
    {
        if (string.IsNullOrEmpty(c.qsToken))
            return;

        // check token
        DataSet dsEmpInfo = empAuth.GetEmployeeDataToLoginByPasswordResetKey(c.qsToken);

        if (dsEmpInfo != null && dsEmpInfo.Tables[0].Rows.Count > 0)
        {
            DataRow drFirst = dsEmpInfo.Tables[0].Rows[0];
            string empAccount = drFirst.ToSafeStr("EmpAccount");

            bool resultCancel = false;

            if (c.qsCancel == "1")
            {
                //取消要求
                resultCancel = empAuth.UpdateEmployeePasswordResetKey(empAccount, "");
                Master.ShowErrorMsg(Resources.Lang.PswChange_ResetPasswordCanceled);
            }

            if (!resultCancel)
            {
                DateTime passwordResetKeyDate = Convert.ToDateTime(drFirst["PasswordResetKeyDate"]);
                TimeSpan tsGap = DateTime.Now - passwordResetKeyDate;

                if (tsGap.TotalHours >= 24)
                {
                    //逾時
                    empAuth.UpdateEmployeePasswordResetKey(empAccount, "");
                    Master.ShowErrorMsg(Resources.Lang.ErrMsg_InvalidTokenOfResetPsw);
                }
                else
                {
                    hidEmpAccountOfToken.Text = empAccount;
                    CurrentPswArea.Visible = false;
                }
            }
        }
        else
        {
            Master.ShowErrorMsg(Resources.Lang.ErrMsg_InvalidTokenOfResetPsw);
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
        Master.ShowErrorMsg("");
        txtCheckCode.Text = "";

        if (!IsValid)
            return;

        txtAccount.Text = txtAccount.Text.Trim();
        txtPassword.Text = txtPassword.Text.Trim();
        txtNewPsw.Text = txtNewPsw.Text.Trim();

        if (string.IsNullOrEmpty(hidEmpAccountOfToken.Text))
        {
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

            //擋 role-guest
            if (drEmpVerify.ToSafeStr("RoleName") == "guest")
            {
                Master.ShowErrorMsg(Resources.Lang.ErrMsg_RoleGuestIsNotAllowedToUse);
                return;
            }

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
        }
        else if (string.Compare(txtAccount.Text, hidEmpAccountOfToken.Text.Trim(), true) != 0)
        {
            Master.ShowErrorMsg(ACCOUNT_FAILED_ERRMSG);
            //新增後端操作記錄
            string description = string.Format("．(變更密碼)來自[{0}]重置密碼連結但是輸入錯誤帳號，輸入值[{1}]　．(change password)From [{0}] reset password link but enter the wrong account! Input[{1}]",
                hidEmpAccountOfToken.Text, txtAccount.Text);

            empAuth.InsertBackEndLogData(new BackEndLogData()
            {
                EmpAccount = "",
                Description = description,
                IP = c.GetClientIP()
            });
            return;
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
        string empAccount = drEmp.ToSafeStr("EmpAccount");
        string empName = drEmp.ToSafeStr("EmpName");
        string email = drEmp.ToSafeStr("Email");

        bool result = empAuth.UpdateEmployeePassword(empAccount, HashUtility.GetPasswordHash(txtNewPsw.Text));

        if (result)
        {
            if (!string.IsNullOrEmpty(hidEmpAccountOfToken.Text))
            {
                //清除Email驗證用唯一值
                empAuth.UpdateEmployeePasswordResetKey(hidEmpAccountOfToken.Text, "");
            }

            //新增後端操作記錄
            empAuth.InsertBackEndLogData(new BackEndLogData()
            {
                EmpAccount = empAccount,
                Description = "．變更密碼　．Change password",
                IP = c.GetClientIP()
            });

            // Email notice
            if (empName.Trim() == "")
                empName = empAccount;

            UserInfo userInfo = new UserInfo()
            {
                EmpAccount = empAccount,
                EmpName = empName,
                Email = email
            };

            bool sentResult = false;

            if (LangManager.Instance.GetCultureName(c.qsLangNo.ToString()) == LangManager.CultureNameZHTW)
            {
                sentResult = SendNoticeMailToUserZhTw(userInfo);
            }
            else
            {
                sentResult = SendNoticeMailToUserEn(userInfo);
            }

            if (!sentResult)
            {
                c.LoggerOfUI.Error(string.Format("Account[{0}] Email[{1}] send notice mail to user failed.", empAccount, email));
            }

            StringBuilder sbScript = new StringBuilder(200);
            sbScript.AppendFormat("window.alert('{0}!');", Resources.Lang.PswChange_Success).AppendLine();
            sbScript.AppendFormat("window.location='{0}?l={1}';", FormsAuthentication.LoginUrl, c.qsLangNo).AppendLine();

            ClientScript.RegisterStartupScript(GetType(), "", sbScript.ToString(), true);
        }
        else
        {
            Master.ShowErrorMsg(Resources.Lang.ErrMsg_ChangePasswordException);
        }
    }

    private bool SendNoticeMailToUserZhTw(UserInfo userInfo)
    {
        bool result = true;
        string smtpServer = ConfigurationManager.AppSettings["SmtpServer"];
        string smtpAccount = ConfigurationManager.AppSettings["SmtpAccount"];
        string smtpPassword = ConfigurationManager.AppSettings["SmtpPassword"];
        string from = string.Format("{0}<{1}>", Resources.Lang.BackStageName, ConfigurationManager.AppSettings["ServiceEmail"]);

        // get template
        StreamReader srTemp = File.OpenText(Server.MapPath("~/" + ConfigurationManager.AppSettings["AttRootDir"] + "MailTemplate/ReplyMailTemplate.htm"));
        string bodyTemplate = srTemp.ReadToEnd();
        srTemp.Close();

        string to = userInfo.Email;

        if (c.IsSendToTesterOnly())
        {
            to = string.Format("原本接收者為 {0}<{1}>", userInfo.Email, ConfigurationManager.AppSettings["TesterEmail"]);
        }

        string cc = "";
        string bcc = "";
        string noticeSubject = string.Format("{0} 您好！提醒您，密碼已變更", userInfo.EmpName);
        string funcUrl = string.Format("{0}/Psw-Require.aspx?l={1}", ConfigurationManager.AppSettings["BackendUrl"], c.qsLangNo);

        string noticeContext = string.Format("{0} 您好，\r\n\r\n　您的帳號為 {1} ，系統於 {2:yyyy/MM/dd HH:mm} 接收到密碼變更要求且完成變更。\r\n\r\n" +
            "為維護您的權益，若此資料異動非本人所認可，請至本系統進行重置密碼。" +
            "<blockquote>重置密碼連結：<a href='{3}'>{3}</a></blockquote>\r\n" +
            "此為系統自動發出的郵件，請勿直接回覆本電子郵件。\r\n\r\n{4} 敬上\r\n",
            userInfo.EmpName, userInfo.EmpAccount, DateTime.Now,
            funcUrl,
            Resources.Lang.BackStageName);

        StringBuilder sbBody = new StringBuilder();
        sbBody.Append(bodyTemplate);
        sbBody.Replace("##header;", Resources.Lang.BackStageName);
        sbBody.Replace("##title;", "密碼已變更");
        sbBody.Replace("##subject;", "密碼已變更");
        sbBody.Replace("##website;", ConfigurationManager.AppSettings["WebsiteUrl"]);
        sbBody.Replace("##cnt;", noticeContext.Replace("\r\n", "<br>"));

        EmailSender emailSender = new EmailSender();
        emailSender.SmtpServer = smtpServer;

        if (c.UseSender())
        {
            emailSender.SmtpServer = ConfigurationManager.AppSettings["SenderSmtpServer"];
            from = ConfigurationManager.AppSettings["SenderEmail"];
            smtpAccount = ConfigurationManager.AppSettings["SenderAccount"];
            smtpPassword = ConfigurationManager.AppSettings["SenderPassword"];
        }

        if (!emailSender.SendHtml(smtpAccount, smtpPassword, from, 
            to, cc, bcc, 
            noticeSubject, sbBody.ToString()))
        {
            result = false;
            userInfo.ErrMsg = emailSender.GetErrMsg();
        }

        return result;
    }

    private bool SendNoticeMailToUserEn(UserInfo userInfo)
    {
        bool result = true;
        string smtpServer = ConfigurationManager.AppSettings["SmtpServer"];
        string smtpAccount = ConfigurationManager.AppSettings["SmtpAccount"];
        string smtpPassword = ConfigurationManager.AppSettings["SmtpPassword"];
        string from = string.Format("{0}<{1}>", Resources.Lang.BackStageName, ConfigurationManager.AppSettings["ServiceEmail"]);

        // get template
        StreamReader srTemp = File.OpenText(Server.MapPath("~/" + ConfigurationManager.AppSettings["AttRootDir"] + "MailTemplate/ReplyMailTemplate.htm"));
        string bodyTemplate = srTemp.ReadToEnd();
        srTemp.Close();

        string to = userInfo.Email;

        if (c.IsSendToTesterOnly())
        {
            to = string.Format("Original recipient is {0}<{1}>", userInfo.Email, ConfigurationManager.AppSettings["TesterEmail"]);
        }

        string cc = "";
        string bcc = "";
        string noticeSubject = string.Format("Please be reminded that your({0}) password has been changed.", userInfo.EmpName);
        string funcUrl = string.Format("{0}/Psw-Require.aspx?l={1}", ConfigurationManager.AppSettings["BackendUrl"], c.qsLangNo);

        string noticeContext = string.Format("Sir/Madam {0}, \r\n\r\nYour account is {1} \r\nThe system received a password modification request at {2:HH:mm, dd/MM/yyyy}, and your password has been changed.\r\n\r\n" +
            "If you have not personally done so, please visit our system to reset the password again." +
            "<blockquote>Password reset link is as follows: <a href='{3}'>{3}</a></blockquote>\r\n" +
            "This is the system automatically sent mail, do not reply directly to this email.\r\n\r\nBest regards,\r\n{4}\r\n",
            userInfo.EmpName, userInfo.EmpAccount, DateTime.Now,
            funcUrl,
            Resources.Lang.BackStageName);

        StringBuilder sbBody = new StringBuilder();
        sbBody.Append(bodyTemplate);
        sbBody.Replace("##header;", Resources.Lang.BackStageName);
        sbBody.Replace("##title;", "Password changed");
        sbBody.Replace("##subject;", "Password changed");
        sbBody.Replace("##website;", ConfigurationManager.AppSettings["WebsiteUrl"]);
        sbBody.Replace("##cnt;", noticeContext.Replace("\r\n", "<br>"));

        EmailSender emailSender = new EmailSender();
        emailSender.SmtpServer = smtpServer;

        if (c.UseSender())
        {
            emailSender.SmtpServer = ConfigurationManager.AppSettings["SenderSmtpServer"];
            from = ConfigurationManager.AppSettings["SenderEmail"];
            smtpAccount = ConfigurationManager.AppSettings["SenderAccount"];
            smtpPassword = ConfigurationManager.AppSettings["SenderPassword"];
        }

        if (!emailSender.SendHtml(smtpAccount, smtpPassword, from,
            to, cc, bcc,
            noticeSubject, sbBody.ToString()))
        {
            result = false;
            userInfo.ErrMsg = emailSender.GetErrMsg();
        }

        return result;
    }

    private class UserInfo
    {
        public string EmpAccount;
        public string EmpName;
        public string Email;
        /// <summary>
        /// return: error message
        /// </summary>
        public string ErrMsg;
    }
}