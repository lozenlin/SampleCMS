using Common.DataAccess;
using Common.DataAccess.EmployeeAuthority;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

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

        /// <summary>
        /// 後台首頁連結位置
        /// </summary>
        public string BACK_END_HOME = "/Management/Dashboard.aspx";

        private string errMsg_MethodNeedsPage = "This method needs to be called in Page.";

        #region qs:=QueryString, se:=Session, vs:=ViewState, co:=Cookie

        /// <summary>
        /// Keyword
        /// </summary>
        public string qsKw
        {
            get { return Request.QueryString["kw"] ?? ""; }
        }

        /// <summary>
        /// 排序欄位名
        /// </summary>
        public string qsSortField
        {
            get { return Request.QueryString["sortfield"] ?? ""; }
        }

        /// <summary>
        /// 為反向排序
        /// </summary>
        public bool qsIsSortDesc
        {
            get
            {
                string str = Request.QueryString["isSortDesc"];
                bool bResult = false;

                if (str != null && bool.TryParse(str, out bResult))
                {
                }
                else
                    return false;

                return bResult;
            }
        }

        /// <summary>
        /// Action of config-form
        /// </summary>
        public string qsAct
        {
            get { return Request.QueryString["act"] ?? ""; }
        }

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

        /// <summary>
        /// 後端語言號碼
        /// </summary>
        public int seLangNoOfBackend
        {
            get
            {
                object obj = Session["seLangNoOfBackend"];
                int nResult = 1;

                if (obj is int)
                {
                    nResult = (int)obj;

                    if (nResult < 1 || nResult > 2)
                        nResult = 1;
                }

                return nResult;
            }

            set { Session["seLangNoOfBackend"] = value; }
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
                return !string.IsNullOrEmpty(seLoginEmpData.RoleName);    //角色識別不為null與空白時,代表已驗證
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

        public void LogOutWhenSessionMissed(string notice)
        {
            Page page = context.CurrentHandler as Page;

            if (page == null)
                throw new Exception(errMsg_MethodNeedsPage);

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
                    .AppendLine("window.open('Logout.ashx', '_top');");

                page.ClientScript.RegisterStartupScript(page.GetType(), "toLogout", sbScript.ToString(), true);
            }
        }

        /// <summary>
        /// 解碼選單網址
        /// </summary>
        public string DecodeUrlOfMenu(string linkUrl)
        {
            if (linkUrl.Contains("{website_url}"))
            {
                linkUrl = linkUrl.Replace("{website_url}", ConfigurationManager.AppSettings["WebsiteUrl"]);
            }
            else if (linkUrl.Contains("{backend_url}"))
            {
                linkUrl = linkUrl.Replace("{backend_url}", ConfigurationManager.AppSettings["BackendUrl"]);
            }

            return linkUrl;
        }

        /// <summary>
        /// 展開選單到指定的功能項目
        /// </summary>
        public void SelectMenuItem(string menuOpId, string menuArticleId)
        {
            Page page = context.CurrentHandler as Page;

            if (page == null)
                throw new Exception(errMsg_MethodNeedsPage);

            page.ClientScript.RegisterStartupScript(this.GetType(), "SelectMenuItem",
                string.Format("$(function(){{ opMenu.initialize('{0}', '{1}'); }});", menuOpId, menuArticleId),
                true);
        }

        /// <summary>
        /// 展開選單到目前頁面的功能項目
        /// </summary>
        public virtual void SelectMenuItemToThisPage()
        {
            string menuOpId = this.GetOpIdOfPage().ToString();
            string menuArticleId = "";

            SelectMenuItem(menuOpId, menuArticleId);
        }

        /// <summary>
        /// 變更至下一個排序狀態
        /// </summary>
        public void ChangeSortStateToNext(ref string sortField, out bool isSortDesc)
        {
            isSortDesc = false;

            if (sortField == qsSortField)
            {
                // default(empty)->asc->desc->default

                if (qsIsSortDesc)
                    sortField = "";
                else
                    isSortDesc = true;
            }
        }

        /// <summary>
        /// 顯示可排序欄位標題區
        /// </summary>
        public void DisplySortableCols(string[] colNames)
        {
            if (qsSortField == "")
                return;

            Control root = context.CurrentHandler as Control;
            string arrowHtml = "";

            if (root == null)
                throw new Exception(errMsg_MethodNeedsPage);

            if (qsIsSortDesc)
                arrowHtml = " <span class='fa fa-chevron-down text-dark'></span>";
            else
                arrowHtml = " <span class='fa fa-chevron-up text-dark'></span>";

            foreach (string colName in colNames)
            {
                LinkButton btnSort = (LinkButton)FindControlRecursive(root, "btnSort" + colName);
                Literal hidText = (Literal)FindControlRecursive(root, "hidSort" + colName);

                if (btnSort.CommandArgument == qsSortField)
                {
                    btnSort.Text = hidText.Text + arrowHtml;
                    string sortHintText = "";

                    if (qsIsSortDesc)
                        sortHintText = "Descending";
                    else
                        sortHintText = "Ascending";

                    btnSort.ToolTip = string.Format("{0} ({1})", hidText.Text, sortHintText);
                }
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

    /// <summary>
    /// 後台帳號管理頁的共用元件
    /// </summary>
    [Description("後台帳號管理頁的共用元件")]
    public class AccountCommonOfBackend : BackendPageCommon, ICustomEmployeeAuthorizationResult
    {
        public AccountCommonOfBackend(HttpContext context, StateBag viewState)
            : base(context, viewState)
        {
        }

        #region qs:=QueryString, se:=Session, vs:=ViewState, co:=Cookie

        /// <summary>
        /// 帳號狀態(0:all, 1:normal, 2:access denied)
        /// </summary>
        public int qsEmpRange
        {
            get
            {
                int nResult;
                string str = Request.QueryString["emprange"];

                if (str != null && int.TryParse(str, out nResult))
                {
                    if (nResult < 0)
                        nResult = 0;
                    else if (nResult > 2)
                        nResult = 2;
                }
                else
                    nResult = 0;

                return nResult;
            }
        }

        /// <summary>
        /// 部門代碼
        /// </summary>
        public int qsDeptId
        {
            get
            {
                string str = Request.QueryString["deptid"];
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
        /// 員工代碼
        /// </summary>
        public int qsEmpId
        {
            get
            {
                string str = Request.QueryString["empid"];
                int nResult = 0;

                if (str != null && int.TryParse(str, out nResult))
                {
                }
                else
                    nResult = 0;

                return nResult;
            }
        }

        #endregion

        public string BuildUrlOfListPage(int emprange, int deptid, string kw, 
            string sortfield, bool isSortDesc, int p)
        {
            return string.Format("Account-List.aspx?emprange={0}&deptid={1}&kw={2}" +
                "&sortfield={3}&isSortDesc={4}&p={5}",
                emprange, deptid, kw,
                sortfield, isSortDesc, p);
        }

        #region ICustomEmployeeAuthorizationResult

        public EmployeeAuthorizationsWithOwnerInfoOfDataExamined InitialAuthorizationResult(bool isTopPageOfOperation, EmployeeAuthorizations authorizations)
        {
            EmployeeAuthorizationsWithOwnerInfoOfDataExamined authAndOwner = new EmployeeAuthorizationsWithOwnerInfoOfDataExamined(authorizations);

            if (!isTopPageOfOperation)
            {
                // get owner info for config-form
                IDataAccessCommand cmd = DataAccessCommandFactory.GetDataAccessCommand(DBs.MainDB);
                spEmployee_GetAccountOfId cmdInfo = new spEmployee_GetAccountOfId()
                {
                    EmpId = qsEmpId
                };

                string errCode = "-1";
                string empAccount = cmd.ExecuteScalar<string>(cmdInfo, errCode);
                string dbErrMsg = cmd.GetErrMsg();

                DataSet ds = null;
                if (empAccount != errCode)
                {
                    spEmployee_GetData cmdInfoGetData = new spEmployee_GetData()
                    {
                        EmpAccount = empAccount
                    };

                    ds = cmd.ExecuteDataset(cmdInfoGetData);
                    dbErrMsg = cmd.GetErrMsg();

                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow drFirst = ds.Tables[0].Rows[0];

                        authAndOwner.OwnerAccountOfDataExamined = drFirst["OwnerAccount"].ToString();
                        authAndOwner.OwnerDeptIdOfDataExamined = Convert.ToInt32(drFirst["OwnerDeptId"]);
                    }
                }
            }

            return authAndOwner;
        }

        #endregion
    }
}
