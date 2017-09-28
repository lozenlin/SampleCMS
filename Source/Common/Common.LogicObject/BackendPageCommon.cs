using System;
using System.Collections.Generic;
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
        /// 帳號
        /// </summary>
        public string seEmpAccount
        {
            get
            {
                return Convert.ToString(Session["EmpAccount"]);
            }
        }

        /// <summary>
        /// 角色識別
        /// </summary>
        public string seRoleName
        {
            get
            {
                return Convert.ToString(Session["RoleName"]);
            }
        }

        /// <summary>
        /// 部門代碼
        /// </summary>
        public int seDeptId
        {
            get
            {
                object obj = Session["DeptId"];
                int result;

                if (obj != null && int.TryParse(obj.ToString(), out result))
                {
                }
                else
                    return 0;

                return result;
            }
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

        #region IAuthenticationConditionProvider

        public int GetOpIdOfPage()
        {
            return opIdOfPage;
        }

        public string GetEmpAccount()
        {
            if (UseFormsAuthentication)
                return User.Identity.Name;
            else
                return seEmpAccount;
        }

        public string GetRoleName()
        {
            return seRoleName;
        }

        public bool IsInRole(string roleName)
        {
            if (UseFormsAuthentication)
                return User.IsInRole(roleName);
            else
                return seRoleName == roleName;
        }

        public int GetDeptId()
        {
            return seDeptId;
        }

        #endregion

    }
}
