using Common.DataAccess;
using Common.DataAccess.EmployeeAuthority;
using System;
using System.Collections.Generic;
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

        /// <summary>
        /// 使用者是否已驗證
        /// </summary>
        public bool IsAuthenticated()
        {
            if (UseFormsAuthentication)
                return User.Identity.IsAuthenticated;
            else
                return seRoleName != "";    //角色識別不為空白時,代表已驗證
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
