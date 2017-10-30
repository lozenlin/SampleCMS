using Common.DataAccess;
using Common.DataAccess.EmployeeAuthority;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;

namespace Common.LogicObject
{
    /// <summary>
    /// 後台網頁的共用元件
    /// </summary>
    public class BackendPageCommon : PageCommon, IAuthenticationConditionProvider
    {
        /// <summary>
        /// 使用Forms驗證: 設定false時,「帳號」和「角色識別」使用Session值
        /// </summary>
        public static bool UseFormsAuthentication = true;

        #region qs:=QueryString, se:=Session, vs:=ViewState, co:=Cookie

        /// <summary>
        /// 登入者資料
        /// </summary>
        public LoginEmployeeData seLoginEmpData
        {
            get
            {
                if (Session["seLoginEmpData"] == null)
                {
                    SaveLoginEmployeeDataIntoSession(new LoginEmployeeData());
                }

                return (LoginEmployeeData)Session["seLoginEmpData"];
            }
        }

        /// <summary>
        /// CAPTCH 驗證碼
        /// </summary>
        public string seCaptchaCode
        {
            get { return Convert.ToString(Session["seCaptchaCode"]); }
            set { Session["seCaptchaCode"] = value; }
        }

        #endregion

        /// <summary>
        /// 後台網頁所屬的作業代碼
        /// </summary>
        protected int opIdOfPage;

        /// <summary>
        /// 後台網頁的共用元件
        /// </summary>
        public BackendPageCommon(HttpContext context, StateBag viewState)
            : base(context, viewState)
        {
        }

        /// <summary>
        /// 使用者是否已驗證
        /// </summary>
        public bool IsAuthenticated()
        {
            if (UseFormsAuthentication)
                return User.Identity.IsAuthenticated;
            else
                return seLoginEmpData.RoleName != "";    //角色識別不為空白時,代表已驗證
        }

        /// <summary>
        /// 用共用元件類別名稱取得後端作業代碼
        /// </summary>
        public int GetOpIdByCommonClass(string commonClass)
        {
            int opId = 0;

            //用共用元件類別名稱取得後端作業選項資訊
            IDataAccessCommand cmd = DataAccessCommandFactory.GetDataAccessCommand(DBs.MainDB);
            spOperations_GetOpInfoByCommonClass cmdInfo = new spOperations_GetOpInfoByCommonClass()
            {
                CommonClass = commonClass
            };

            DataSet dsOp = cmd.ExecuteDataset(cmdInfo);

            if (dsOp != null && dsOp.Tables[0].Rows.Count > 0)
            {
                DataRow drOp = dsOp.Tables[0].Rows[0];
                opId = Convert.ToInt32(drOp["OpId"]);
            }

            return opId;
        }

        public void SaveLoginEmployeeDataIntoSession(LoginEmployeeData loginEmpData)
        {
            Session["seLoginEmpData"] = loginEmpData;
        }

        public void LogOutWhenSessionMissed(Page webPage, string notice)
        {
            if (seLoginEmpData.EmpAccount == null)
            {
                if (UseFormsAuthentication)
                {
                    System.Web.Security.FormsAuthentication.SignOut();
                }

                StringBuilder sbScript = new StringBuilder(200);
                sbScript
                    .AppendFormat("window.alert('{0}');", notice)
                    .AppendLine()
                    .AppendLine("window.open('Logout.ashx?l=" + qsLangNo + "', '_top');");

                webPage.ClientScript.RegisterStartupScript(webPage.GetType(), "toLogout", sbScript.ToString(), true);
            }
        }

        #region IAuthenticationConditionProvider

        /// <summary>
        /// 取得後台網頁所屬的作業代碼
        /// </summary>
        public virtual int GetOpIdOfPage()
        {
            if (opIdOfPage < 1)
            {
                string commonClass = this.GetType().Name;
                opIdOfPage = GetOpIdByCommonClass(commonClass);
            }

            return opIdOfPage;
        }

        public string GetEmpAccount()
        {
            if (UseFormsAuthentication)
                return User.Identity.Name;
            else
                return seLoginEmpData.EmpAccount;
        }

        public string GetRoleName()
        {
            return seLoginEmpData.RoleName;
        }

        public bool IsInRole(string roleName)
        {
            if (UseFormsAuthentication)
                return User.IsInRole(roleName);
            else
                return seLoginEmpData.RoleName == roleName;
        }

        public int GetDeptId()
        {
            return seLoginEmpData.DeptId;
        }

        #endregion
    }

    /// <summary>
    /// 後台登入頁的共用元件
    /// </summary>
    [Description("後台登入頁的共用元件")]
    public class LoginCommonOfBackend : BackendPageCommon
    {
        public LoginCommonOfBackend(HttpContext context, StateBag viewState)
            : base(context, viewState)
        {
        }

        #region qs:=QueryString, se:=Session, vs:=ViewState, co:=Cookie

        /// <summary>
        /// 登入失敗次數
        /// </summary>
        public int seLoginFailedCount
        {
            get
            {
                object obj = Session["seLoginFailedCount"];
                int nResult = 0;
                if (obj != null)
                {
                    if (!int.TryParse(obj.ToString(), out nResult))
                        nResult = 999999999;
                }

                return nResult;
            }

            set { Session["seLoginFailedCount"] = value; }
        }

        #endregion

    }
}
