using Common.LogicObject;
using Common.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Psw_Require : System.Web.UI.Page
{
    protected LoginCommonOfBackend c;
    protected EmployeeAuthorityLogic empAuth;

    private string ACCOUNT_FAILED_ERRMSG = "";

    protected void Page_PreInit(object sender, EventArgs e)
    {
        c = new LoginCommonOfBackend(this.Context, this.ViewState);
        c.InitialLoggerOfUI(this.GetType());
        empAuth = new EmployeeAuthorityLogic();

        ACCOUNT_FAILED_ERRMSG = Resources.Lang.ErrMsg_NoMatchedAccount;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadUIData();
        }

        Title = Resources.Lang.PswRequire_Subtitle + " - " + Title;
    }

    private void LoadUIData()
    {
        txtAccount.Focus();

        revEmail.ErrorMessage = "*" + Resources.Lang.ErrMsg_WrongFormat;
        btnSubmit.ToolTip = Resources.Lang.Login_btnSubmit;
        btnBackToLogin.Title = Resources.Lang.Login_btnBackToLogin;
        cuvCheckCode.ErrorMessage = "*" + Resources.Lang.ErrMsg_WrongCheckCode;
        btnRefreshCodePic.Title = Resources.Lang.CaptchaPic_Hint;
        btnBackToLogin.HRef = StringUtility.SetParaValueInUrl(FormsAuthentication.LoginUrl, "l", c.qsLangNo.ToString());
    }

    protected void cuvCheckCode_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = false;

        if (c.seCaptchaCode == "")
            args.IsValid = false;
        else if (txtCheckCode.Text.ToLower() == c.seCaptchaCode.ToLower())
            args.IsValid = true;
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        Master.ShowErrorMsg("");
        txtCheckCode.Text = "";

        if (!IsValid)
            return;

        txtAccount.Text = txtAccount.Text.Trim();
        txtEmail.Text = txtEmail.Text.Trim();
        
        // check account
        DataSet dsEmp = empAuth.GetEmployeeData(txtAccount.Text);

        if (dsEmp == null || dsEmp.Tables[0].Rows.Count == 0)
        {
            Master.ShowErrorMsg(ACCOUNT_FAILED_ERRMSG);
            //新增後端操作記錄
            empAuth.InsertBackEndLogData(new BackEndLogData()
            {
                EmpAccount = "",
                Description = string.Format("．(要求重置密碼)帳號不存在，輸入帳號[{0}]　．(requires reset password)Account doesn't exist! Account[{0}]", txtAccount.Text),
                IP = c.GetClientIP()
            });
            return;
        }

        DataRow drEmpVerify = dsEmp.Tables[0].Rows[0];

        //擋 role-guest
        if (drEmpVerify.ToSafeStr("RoleName") == "guest")
        {
            Master.ShowErrorMsg(Resources.Lang.ErrMsg_RoleGuestIsNotAllowedToUse);
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
                Description = string.Format("．(要求重置密碼)帳號停用，帳號[{0}]　．(requires reset password)Account is denied! Account[{0}]", txtAccount.Text),
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
                    Description = string.Format("．(要求重置密碼)帳號超出有效範圍，帳號[{0}]　．(requires reset password)Account validation date is out of range! Account[{0}]", txtAccount.Text),
                    IP = c.GetClientIP()
                });
                return;
            }
        }

        DataRow drEmp = dsEmp.Tables[0].Rows[0];
        string empAccount = drEmp.ToSafeStr("EmpAccount");
        string empName = drEmp.ToSafeStr("EmpName");
        string email = drEmp.ToSafeStr("Email");

        // check email
        if (string.Compare(txtEmail.Text, email, true) != 0)
        {
            Master.ShowErrorMsg(ACCOUNT_FAILED_ERRMSG);
            //新增後端操作記錄
            empAuth.InsertBackEndLogData(new BackEndLogData()
            {
                EmpAccount = "",
                Description = string.Format("．(要求重置密碼)Email 不正確，輸入帳號[{0}] Email[{1}]　．(requires reset password)Wrong Email! Account[{0}] Email[{1}]", txtAccount.Text, txtEmail.Text),
                IP = c.GetClientIP()
            });
            return;
        }

        //產生驗證用唯一值
        string passwordResetKey = Guid.NewGuid().ToString();
        bool result = false;
        result = empAuth.UpdateEmployeePasswordResetKey(empAccount, passwordResetKey);

        if (result)
        {
            //新增後端操作記錄
            empAuth.InsertBackEndLogData(new BackEndLogData()
            {
                EmpAccount = empAccount,
                Description = "．要求重置密碼　．Requires reset password",
                IP = c.GetClientIP()
            });

            // Email notice
            if (empName.Trim() == "")
                empName = empAccount;

            UserInfo userInfo = new UserInfo()
            {
                EmpAccount = empAccount,
                EmpName = empName,
                Email = email,
                EmailConfirmKey = passwordResetKey
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

            if (sentResult)
            {
                StringBuilder sbScript = new StringBuilder(200);
                sbScript.AppendFormat("window.alert('{0}!');", Resources.Lang.PswRequire_Success).AppendLine();
                sbScript.AppendFormat("window.location='{0}?l={1}';", FormsAuthentication.LoginUrl, c.qsLangNo).AppendLine();

                ClientScript.RegisterStartupScript(GetType(), "", sbScript.ToString(), true);
            }
            else
            {
                c.LoggerOfUI.Error(string.Format("Account[{0}] Email[{1}] send notice mail to user failed.", empAccount, email));
                Master.ShowErrorMsg(Resources.Lang.ErrMsg_PswRequireSendFailed);
            }
        }
        else
        {
            Master.ShowErrorMsg(Resources.Lang.ErrMsg_PrepareEmailException);
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
        string noticeSubject = "重置密碼連結";
        string funcUrl = string.Format("{0}/Psw-Change.aspx?token={1}&l={2}",
                            ConfigurationManager.AppSettings["BackendUrl"], Server.UrlEncode(userInfo.EmailConfirmKey), c.qsLangNo);
        string cancelUrl = funcUrl + "&cancel=1";

        string noticeContext = string.Format("{0} 您好，\r\n\r\n　您的帳號為 {1} ，系統於 {2:yyyy/MM/dd HH:mm} 接收到重置密碼要求。\r\n" +
            "<blockquote>重置密碼連結：<a href='{3}'>{3}</a></blockquote>\r\n" +
            "若此要求非本人所認可或已不需要，連結將於 24 小時後失效，或點選取消要求的連結。\r\n" +
            "<blockquote>取消要求連結：<a href='{4}'>{4}</a></blockquote>\r\n" +
            "此為系統自動發出的郵件，請勿直接回覆本電子郵件。\r\n\r\n{5} 敬上\r\n", 
            userInfo.EmpName, userInfo.EmpAccount, DateTime.Now,
            funcUrl,
            cancelUrl,
            Resources.Lang.BackStageName);

        StringBuilder sbBody = new StringBuilder();
        sbBody.Append(bodyTemplate);
        sbBody.Replace("##header;", Resources.Lang.BackStageName);
        sbBody.Replace("##title;", "要求重置密碼");
        sbBody.Replace("##subject;", "要求重置密碼");
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
            to = string.Format("原本接收者為 {0}<{1}>", userInfo.Email, ConfigurationManager.AppSettings["TesterEmail"]);
        }

        string cc = "";
        string bcc = "";
        string noticeSubject = Resources.Lang.PswRequire_Subtitle;
        string funcUrl = string.Format("{0}/Psw-Change.aspx?token={1}&l={2}",
                            ConfigurationManager.AppSettings["BackendUrl"], Server.UrlEncode(userInfo.EmailConfirmKey), c.qsLangNo);
        string cancelUrl = funcUrl + "&cancel=1";

        string noticeContext = string.Format("Sir/Madam {0}, \r\n\r\nYour account is {1} .\r\nThe system received a password reset request at {2:HH:mm, dd/MM/yyyy}.\r\n" +
            "<blockquote>The reset link is as follows: <a href='{3}'>{3}</a></blockquote>\r\n" +
            "If you have not personally made such a request or do not need it anymore, please click on the cancellation link. The link will expire after 24 hours.\r\n" +
            "<blockquote>The cancellation link is as follows: <a href='{4}'>{4}</a></blockquote>\r\n" +
            "This is the system automatically sent mail, do not reply directly to this email.\r\n\r\nBest regards,\r\n{5}\r\n",
            userInfo.EmpName, userInfo.EmpAccount, DateTime.Now,
            funcUrl,
            cancelUrl,
            Resources.Lang.BackStageName);

        StringBuilder sbBody = new StringBuilder();
        sbBody.Append(bodyTemplate);
        sbBody.Replace("##header;", Resources.Lang.BackStageName);
        sbBody.Replace("##title;", Resources.Lang.PswRequire_Subtitle);
        sbBody.Replace("##subject;", Resources.Lang.PswRequire_Subtitle);
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
        public string EmailConfirmKey;
        /// <summary>
        /// return: error message
        /// </summary>
        public string ErrMsg;
    }
}