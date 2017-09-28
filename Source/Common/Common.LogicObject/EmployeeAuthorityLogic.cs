using Common.DataAccess;
using Common.DataAccess.EmployeeAuthority;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Common.LogicObject
{
    /// <summary>
    /// 帳號與權限
    /// </summary>
    public class EmployeeAuthorityLogic
    {
        #region 授權屬性 Authorizations

        /// <summary>
        /// 目標資料的擁有者帳號
        /// </summary>
        public string OwnerAccountOfDataExamined
        {
            get { return ownerAccountOfDataExamined; }
            set { ownerAccountOfDataExamined = value; }
        }
        protected string ownerAccountOfDataExamined = "";

        /// <summary>
        /// 目標資料的擁有者部門
        /// </summary>
        public int OwnerDeptIdOfDataExamined
        {
            get { return ownerDeptIdOfDataExamined; }
            set { ownerDeptIdOfDataExamined = value; }
        }
        protected int ownerDeptIdOfDataExamined = 0;

        /// <summary>
        /// 可閱讀該項目
        /// </summary>
        public bool CanRead
        {
            get { return canRead; }
        }
        protected bool canRead = false;

        /// <summary>
        /// 可修改該項目
        /// </summary>
        public bool CanEdit
        {
            get { return canEdit; }
        }
        protected bool canEdit = false;

        /// <summary>
        /// 可閱讀自己的子項目
        /// </summary>
        public bool CanReadSubItemOfSelf
        {
            get { return canReadSubItemOfSelf; }
        }
        protected bool canReadSubItemOfSelf = false;

        /// <summary>
        /// 可修改自己的子項目
        /// </summary>
        public bool CanEditSubItemOfSelf
        {
            get { return canEditSubItemOfSelf; }
        }
        protected bool canEditSubItemOfSelf = false;

        /// <summary>
        /// 可新增自己的子項目
        /// </summary>
        public bool CanAddSubItemOfSelf
        {
            get { return canAddSubItemOfSelf; }
        }
        protected bool canAddSubItemOfSelf = false;

        /// <summary>
        /// 可刪除自己的子項目
        /// </summary>
        public bool CanDelSubItemOfSelf
        {
            get { return canDelSubItemOfSelf; }
        }
        protected bool canDelSubItemOfSelf = false;

        /// <summary>
        /// 可閱讀同部門的子項目
        /// </summary>
        public bool CanReadSubItemOfCrew
        {
            get { return canReadSubItemOfCrew; }
        }
        protected bool canReadSubItemOfCrew = false;

        /// <summary>
        /// 可修改同部門的子項目
        /// </summary>
        public bool CanEditSubItemOfCrew
        {
            get { return canEditSubItemOfCrew; }
        }
        protected bool canEditSubItemOfCrew = false;

        /// <summary>
        /// 可刪除同部門的子項目
        /// </summary>
        public bool CanDelSubItemOfCrew
        {
            get { return canDelSubItemOfCrew; }
        }
        protected bool canDelSubItemOfCrew = false;

        /// <summary>
        /// 可閱讀任何人的子項目
        /// </summary>
        public bool CanReadSubItemOfOthers
        {
            get { return canReadSubItemOfOthers; }
        }
        protected bool canReadSubItemOfOthers = false;

        /// <summary>
        /// 可修改任何人的子項目
        /// </summary>
        public bool CanEditSubItemOfOthers
        {
            get { return canEditSubItemOfOthers; }
        }
        protected bool canEditSubItemOfOthers = false;

        /// <summary>
        /// 可刪除任何人的子項目
        /// </summary>
        public bool CanDelSubItemOfOthers
        {
            get { return canDelSubItemOfOthers; }
        }
        protected bool canDelSubItemOfOthers = false;

        #endregion

        protected IDataAccessSource db;
        /// <summary>
        /// 為作業項目中的最上層頁面
        /// </summary>
        protected bool isTopPageOfOperation = true;
        protected int opIdOfPage = 0;
        protected string empAccount = "";
        protected string roleName = "";
        protected bool isRoleAdmin = false;
        protected int deptId = 0;

        /// <summary>
        /// 帳號與權限
        /// </summary>
        public EmployeeAuthorityLogic(IAuthenticationConditionProvider authCondition, IDataAccessSource db)
        {
            opIdOfPage = authCondition.GetOpIdOfPage();
            empAccount = authCondition.GetEmpAccount();
            roleName = authCondition.GetRoleName();
            isRoleAdmin = authCondition.IsInRole("admin");
            deptId = authCondition.GetDeptId();
        }

        /// <summary>
        /// 初始化最上層頁面授權結果
        /// </summary>
        public void InitialAuthorizationResultOfTopPage()
        {
            InitialAuthorizationResult(true);
        }

        /// <summary>
        /// 初始化下層頁面授權結果
        /// </summary>
        public void InitialAuthorizationResultOfSubPages()
        {
            InitialAuthorizationResult(false);
        }

        /// <summary>
        /// 初始化授權結果
        /// </summary>
        protected virtual void InitialAuthorizationResult(bool isTopPageOfOperation)
        {
            this.isTopPageOfOperation = isTopPageOfOperation;

            //取得指定作業代碼的後端角色可使用權限
            IDataAccessCommand cmd = DataAccessCommandFactory.GetDataAccessCommand(db);
            spEmployeeRoleOperationsDesc_GetDataOfOp cmdInfo = new spEmployeeRoleOperationsDesc_GetDataOfOp()
            {
                OpId = opIdOfPage,
                RoleName = roleName
            };

            DataSet dsRoleOp = cmd.ExecuteDataset(cmdInfo);

            //從資料集載入角色的授權設定
            LoadRoleAuthorizationsFrom(dsRoleOp);
        }

        /// <summary>
        /// 從資料集載入角色的授權設定
        /// </summary>
        public bool LoadRoleAuthorizationsFrom(DataSet dsRoleOp)
        {
            if (isRoleAdmin)
            {
                //管理者,權限最大
                canRead = true;
                canEdit = true;

                canReadSubItemOfSelf = true;
                canEditSubItemOfSelf = true;
                canAddSubItemOfSelf = true;
                canDelSubItemOfSelf = true;

                canReadSubItemOfCrew = true;
                canEditSubItemOfCrew = true;
                canDelSubItemOfCrew = true;

                canReadSubItemOfOthers = true;
                canEditSubItemOfOthers = true;
                canDelSubItemOfOthers = true;
            }
            else
            {
                if (dsRoleOp == null || dsRoleOp.Tables[0].Rows.Count == 0)
                    return false;

                DataRow drRoleOp = dsRoleOp.Tables[0].Rows[0];

                //載入設定
                canRead = Convert.ToBoolean(drRoleOp["CanRead"]);
                canEdit = Convert.ToBoolean(drRoleOp["CanEdit"]);

                canReadSubItemOfSelf = Convert.ToBoolean(drRoleOp["CanReadSubItemOfSelf"]);
                canEditSubItemOfSelf = Convert.ToBoolean(drRoleOp["CanEditSubItemOfSelf"]);
                canAddSubItemOfSelf = Convert.ToBoolean(drRoleOp["CanAddSubItemOfSelf"]);
                canDelSubItemOfSelf = Convert.ToBoolean(drRoleOp["CanDelSubItemOfSelf"]);

                canReadSubItemOfCrew = Convert.ToBoolean(drRoleOp["CanReadSubItemOfCrew"]);
                canEditSubItemOfCrew = Convert.ToBoolean(drRoleOp["CanEditSubItemOfCrew"]);
                canDelSubItemOfCrew = Convert.ToBoolean(drRoleOp["CanDelSubItemOfCrew"]);

                canReadSubItemOfOthers = Convert.ToBoolean(drRoleOp["CanReadSubItemOfOthers"]);
                canEditSubItemOfOthers = Convert.ToBoolean(drRoleOp["CanEditSubItemOfOthers"]);
                canDelSubItemOfOthers = Convert.ToBoolean(drRoleOp["CanDelSubItemOfOthers"]);
            }

            return true;
        }

        /// <summary>
        /// 檢查是否可開啟此頁面
        /// </summary>
        public virtual bool CanOpenThisPage()
        {
            bool result = false;

            if (isTopPageOfOperation)
            {
                result = canRead;
            }
            else
            {
                if (canReadSubItemOfOthers)
                {
                    result = true;
                }
                else if (CanReadSubItemOfCrew
                    && deptId > 0
                    && deptId == ownerDeptIdOfDataExamined)
                {
                    result = true;
                }
                else if (canReadSubItemOfSelf 
                    && empAccount != "" 
                    && empAccount == ownerAccountOfDataExamined)
                {
                    result = true;
                }
            }

            return result;
        }

        /// <summary>
        /// 檢查是否可編輯此頁面
        /// </summary>
        public virtual bool CanEditThisPage()
        {
            return CanEditThisPage(isTopPageOfOperation, ownerAccountOfDataExamined, ownerDeptIdOfDataExamined);
        }

        /// <summary>
        /// 檢查是否可編輯此頁面
        /// </summary>
        public virtual bool CanEditThisPage(bool useTopRule, string ownerAccount, int ownerDeptId)
        {
            bool result = false;

            if (useTopRule)
            {
                result = CanEdit;
            }
            else
            {
                if (CanEditSubItemOfOthers)
                {
                    result = true;
                }
                else if (CanEditSubItemOfCrew
                    && deptId > 0
                    && deptId == ownerDeptIdOfDataExamined)
                {
                    result = true;
                }
                else if (CanEditSubItemOfSelf
                    && empAccount != ""
                    && empAccount == ownerAccountOfDataExamined)
                {
                    result = true;
                }
            }

            return result;
        }

        /// <summary>
        /// 檢查是否可刪除此頁面
        /// </summary>
        public virtual bool CanDelThisPage(string ownerAccount, int ownerDeptId)
        {
            bool result = false;

            if (CanDelSubItemOfOthers)
            {
                result = true;
            }
            else if (CanDelSubItemOfCrew
                && deptId > 0
                && deptId == ownerDeptIdOfDataExamined)
            {
                result = true;
            }
            else if (CanDelSubItemOfSelf
                && empAccount != ""
                && empAccount == ownerAccountOfDataExamined)
            {
                result = true;
            }

            return result;
        }

    }
}
