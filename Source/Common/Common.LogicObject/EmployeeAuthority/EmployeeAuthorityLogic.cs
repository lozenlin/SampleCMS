// ===============================================================================
// EmployeeAuthorityLogic of SampleCMS
// https://github.com/lozenlin/SampleCMS
//
// EmployeeAuthorityLogic.cs
//
// ===============================================================================
// Copyright (c) 2017 lozenlin
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// ===============================================================================

using Common.DataAccess;
using Common.DataAccess.EmployeeAuthority;
using log4net;
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

        protected IAuthenticationConditionProvider authCondition;
        protected EmployeeAuthorizations authorizations = null;
        protected ILog logger = null;
        /// <summary>
        /// 為作業項目中的最上層頁面
        /// </summary>
        protected bool isTopPageOfOperation = true;
        protected int opIdOfPage = 0;
        protected string empAccount = "";
        protected string roleName = "";
        protected bool isRoleAdmin = false;
        protected int deptId = 0;
        protected string dbErrMsg = "";

        /// <summary>
        /// 帳號與權限
        /// </summary>
        public EmployeeAuthorityLogic(IAuthenticationConditionProvider authCondition)
        {
            this.authCondition = authCondition;
            this.authorizations = new EmployeeAuthorizations();
            logger = LogManager.GetLogger(this.GetType());
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

            //取得指定作業代碼的後端身分可使用權限
            IDataAccessCommand cmd = DataAccessCommandFactory.GetDataAccessCommand(DBs.MainDB);
            spEmployeeRoleOperationsDesc_GetDataOfOp cmdInfo = new spEmployeeRoleOperationsDesc_GetDataOfOp()
            {
                OpId = opIdOfPage,
                RoleName = roleName
            };

            DataSet dsRoleOp = cmd.ExecuteDataset(cmdInfo);

            //從資料集載入身分的授權設定
            LoadRoleAuthorizationsFrom(dsRoleOp);

            if (authCondition is ICustomEmployeeAuthorizationResult)
            {
                //自訂帳號授權結果
                EmployeeAuthorizationsWithOwnerInfoOfDataExamined authAndOwner = ((ICustomEmployeeAuthorizationResult)authCondition).InitialAuthorizationResult(isTopPageOfOperation, authorizations);
                ownerAccountOfDataExamined = authAndOwner.OwnerAccountOfDataExamined;
                ownerDeptIdOfDataExamined = authAndOwner.OwnerDeptIdOfDataExamined;
                this.authorizations = authAndOwner;
                return;
            }
        }

        /// <summary>
        /// 從資料集載入身分的授權設定
        /// </summary>
        public bool LoadRoleAuthorizationsFrom(DataSet dsRoleOp)
        {
            if (isRoleAdmin)
            {
                //管理者,權限最大
                authorizations.CanRead = true;
                authorizations.CanEdit = true;

                authorizations.CanReadSubItemOfSelf = true;
                authorizations.CanEditSubItemOfSelf = true;
                authorizations.CanAddSubItemOfSelf = true;
                authorizations.CanDelSubItemOfSelf = true;

                authorizations.CanReadSubItemOfCrew = true;
                authorizations.CanEditSubItemOfCrew = true;
                authorizations.CanDelSubItemOfCrew = true;

                authorizations.CanReadSubItemOfOthers = true;
                authorizations.CanEditSubItemOfOthers = true;
                authorizations.CanDelSubItemOfOthers = true;
            }
            else
            {
                if (dsRoleOp == null || dsRoleOp.Tables[0].Rows.Count == 0)
                {
                    logger.Info("parameter of LoadRoleAuthorizationsFrom() is empty.");
                    return false;
                }

                DataRow drRoleOp = dsRoleOp.Tables[0].Rows[0];

                //載入設定
                authorizations.CanRead = Convert.ToBoolean(drRoleOp["CanRead"]);
                authorizations.CanEdit = Convert.ToBoolean(drRoleOp["CanEdit"]);

                authorizations.CanReadSubItemOfSelf = Convert.ToBoolean(drRoleOp["CanReadSubItemOfSelf"]);
                authorizations.CanEditSubItemOfSelf = Convert.ToBoolean(drRoleOp["CanEditSubItemOfSelf"]);
                authorizations.CanAddSubItemOfSelf = Convert.ToBoolean(drRoleOp["CanAddSubItemOfSelf"]);
                authorizations.CanDelSubItemOfSelf = Convert.ToBoolean(drRoleOp["CanDelSubItemOfSelf"]);

                authorizations.CanReadSubItemOfCrew = Convert.ToBoolean(drRoleOp["CanReadSubItemOfCrew"]);
                authorizations.CanEditSubItemOfCrew = Convert.ToBoolean(drRoleOp["CanEditSubItemOfCrew"]);
                authorizations.CanDelSubItemOfCrew = Convert.ToBoolean(drRoleOp["CanDelSubItemOfCrew"]);

                authorizations.CanReadSubItemOfOthers = Convert.ToBoolean(drRoleOp["CanReadSubItemOfOthers"]);
                authorizations.CanEditSubItemOfOthers = Convert.ToBoolean(drRoleOp["CanEditSubItemOfOthers"]);
                authorizations.CanDelSubItemOfOthers = Convert.ToBoolean(drRoleOp["CanDelSubItemOfOthers"]);
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
                result = authorizations.CanRead;
            }
            else
            {
                if (authorizations.CanReadSubItemOfOthers)
                {
                    result = true;
                }
                else if (authorizations.CanReadSubItemOfCrew
                    && deptId > 0
                    && deptId == ownerDeptIdOfDataExamined)
                {
                    result = true;
                }
                else if (authorizations.CanReadSubItemOfSelf 
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
                result = authorizations.CanEdit;
            }
            else
            {
                if (authorizations.CanEditSubItemOfOthers)
                {
                    result = true;
                }
                else if (authorizations.CanEditSubItemOfCrew
                    && deptId > 0
                    && deptId == ownerDeptId)
                {
                    result = true;
                }
                else if (authorizations.CanEditSubItemOfSelf
                    && empAccount != ""
                    && empAccount == ownerAccount)
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

            if (authorizations.CanDelSubItemOfOthers)
            {
                result = true;
            }
            else if (authorizations.CanDelSubItemOfCrew
                && deptId > 0
                && deptId == ownerDeptId)
            {
                result = true;
            }
            else if (authorizations.CanDelSubItemOfSelf
                && empAccount != ""
                && empAccount == ownerAccount)
            {
                result = true;
            }

            return result;
        }

        /// <summary>
        /// 檢查是否可在此頁面新增子項目
        /// </summary>
        public virtual bool CanAddSubItemInThisPage()
        {
            return authorizations.CanAddSubItemOfSelf;
        }

        /// <summary>
        /// 可閱讀任何人的子項目
        /// </summary>
        public bool CanReadSubItemOfOthers()
        {
            return authorizations.CanReadSubItemOfOthers;
        }

        /// <summary>
        /// 可閱讀同部門的子項目
        /// </summary>
        public bool CanReadSubItemOfCrew()
        {
            return authorizations.CanReadSubItemOfCrew;
        }

        /// <summary>
        /// 可閱讀自己的子項目
        /// </summary>
        public bool CanReadSubItemOfSelf()
        {
            return authorizations.CanReadSubItemOfSelf;
        }

        // DataAccess functions

        /// <summary>
        /// DB command 執行後的錯誤訊息
        /// </summary>
        public string GetDbErrMsg()
        {
            return dbErrMsg;
        }

        #region BackEndLog DataAccess functions

        /// <summary>
        /// 新增後端操作記錄
        /// </summary>
        public bool InsertBackEndLogData(BackEndLogData data)
        {
            IDataAccessCommand cmd = DataAccessCommandFactory.GetDataAccessCommand(DBs.MainDB);
            spBackEndLog_InsertData cmdInfo = new spBackEndLog_InsertData()
            {
                EmpAccount = data.EmpAccount,
                Description = data.Description,
                IP = data.IP
            };

            bool result = cmd.ExecuteNonQuery(cmdInfo);
            dbErrMsg = cmd.GetErrMsg();

            return result;
        }

        #endregion

        #region Employee DataAccess functions

        /// <summary>
        /// 取得員工登入用資料
        /// </summary>
        public DataSet GetEmployeeDataToLogin(string empAccount)
        {
            IDataAccessCommand cmd = DataAccessCommandFactory.GetDataAccessCommand(DBs.MainDB);
            spEmployee_GetDataToLogin cmdInfo = new spEmployee_GetDataToLogin()
            {
                EmpAccount = empAccount
            };

            DataSet ds = cmd.ExecuteDataset(cmdInfo);
            dbErrMsg = cmd.GetErrMsg();

            return ds;
        }

        /// <summary>
        /// 取得員工資料
        /// </summary>
        public DataSet GetEmployeeData(string empAccount)
        {
            IDataAccessCommand cmd = DataAccessCommandFactory.GetDataAccessCommand(DBs.MainDB);
            spEmployee_GetData cmdInfo = new spEmployee_GetData()
            {
                EmpAccount = empAccount
            };

            DataSet ds = cmd.ExecuteDataset(cmdInfo);
            dbErrMsg = cmd.GetErrMsg();

            return ds;
        }

        /// <summary>
        /// 取得員工資料
        /// </summary>
        public DataSet GetEmployeeData(int empId)
        {
            IDataAccessCommand cmd = DataAccessCommandFactory.GetDataAccessCommand(DBs.MainDB);
            spEmployee_GetAccountOfId cmdInfo = new spEmployee_GetAccountOfId()
            {
                EmpId = empId
            };

            string errCode = "-1";
            string empAccount = cmd.ExecuteScalar<string>(cmdInfo, errCode);
            dbErrMsg = cmd.GetErrMsg();

            DataSet ds = null;
            if (empAccount != errCode)
            {
                ds = GetEmployeeData(empAccount);
            }

            return ds;
        }

        /// <summary>
        /// 取得員工身分名稱
        /// </summary>
        public string GetRoleNameOfEmp(string empAccount)
        {
            string roleName = "";

            IDataAccessCommand cmd = DataAccessCommandFactory.GetDataAccessCommand(DBs.MainDB);
            spEmployee_GetRoleName cmdInfo = new spEmployee_GetRoleName()
            {
                EmpAccount = empAccount
            };
            roleName = cmd.ExecuteScalar<string>(cmdInfo, "-1");
            dbErrMsg = cmd.GetErrMsg();

            if (roleName == "-1")
            {
                roleName = "";
            }

            return roleName;
        }

        /// <summary>
        /// 更新員工本次登入資訊
        /// </summary>
        public bool UpdateEmployeeLoginInfo(string empAccount, string thisLoginIP)
        {
            IDataAccessCommand cmd = DataAccessCommandFactory.GetDataAccessCommand(DBs.MainDB);
            spEmployee_UpdateLoginInfo cmdInfo = new spEmployee_UpdateLoginInfo()
            {
                EmpAccount = empAccount,
                ThisLoginIP = thisLoginIP
            };
            bool result = cmd.ExecuteNonQuery(cmdInfo);
            dbErrMsg = cmd.GetErrMsg();

            return result;
        }

        /// <summary>
        /// 取得帳號名單
        /// </summary>
        public DataSet GetAccountList(AccountListQueryParams param)
        {
            IDataAccessCommand cmd = DataAccessCommandFactory.GetDataAccessCommand(DBs.MainDB);
            spEmployee_GetList cmdInfo = new spEmployee_GetList()
            {
                DeptId = param.DeptId,
                Kw = param.Kw,
                ListMode = param.ListMode,
                BeginNum = param.PagedParams.BeginNum,
                EndNum = param.PagedParams.EndNum,
                SortField = param.PagedParams.SortField,
                IsSortDesc = param.PagedParams.IsSortDesc,
                CanReadSubItemOfOthers = param.AuthParams.CanReadSubItemOfOthers,
                CanReadSubItemOfCrew = param.AuthParams.CanReadSubItemOfCrew,
                CanReadSubItemOfSelf = param.AuthParams.CanReadSubItemOfSelf,
                MyAccount = param.AuthParams.MyAccount,
                MyDeptId = param.AuthParams.MyDeptId,
                RowCount = param.PagedParams.RowCount
            };
            DataSet ds = cmd.ExecuteDataset(cmdInfo);
            dbErrMsg = cmd.GetErrMsg();
            param.PagedParams.RowCount = cmdInfo.RowCount;

            return ds;
        }

        /// <summary>
        /// 刪除員工資料
        /// </summary>
        public bool DeleteEmployeeData(int empId)
        {
            IDataAccessCommand cmd = DataAccessCommandFactory.GetDataAccessCommand(DBs.MainDB);
            spEmployee_DeleteData cmdInfo = new spEmployee_DeleteData()
            {
                EmpId = empId
            };
            bool result = cmd.ExecuteNonQuery(cmdInfo);
            dbErrMsg = cmd.GetErrMsg();

            return result;
        }

        /// <summary>
        /// 新增員工資料
        /// </summary>
        public bool InsertEmployeeData(AccountParams param)
        {
            IDataAccessCommand cmd = DataAccessCommandFactory.GetDataAccessCommand(DBs.MainDB);
            spEmployee_InsertData cmdInfo = new spEmployee_InsertData()
            {
                EmpAccount = param.EmpAccount,
                EmpPassword = param.EmpPassword,
                EmpName = param.EmpName,
                Email = param.Email,
                Remarks = param.Remarks,
                DeptId = param.DeptId,
                RoleId = param.RoleId,
                IsAccessDenied = param.IsAccessDenied,
                StartDate = param.StartDate,
                EndDate = param.EndDate,
                OwnerAccount = param.OwnerAccount,
                PasswordHashed = param.PasswordHashed,
                DefaultRandomPassword = param.DefaultRandomPassword,
                PostAccount = param.PostAccount
            };
            bool result = cmd.ExecuteNonQuery(cmdInfo);
            dbErrMsg = cmd.GetErrMsg();

            if (result)
            {
                param.EmpId = cmdInfo.EmpId;
            }
            else if (cmd.GetSqlErrNumber() == 50000 && cmd.GetSqlErrState() == 2)
            {
                param.HasAccountBeenUsed = true;
            }

            return result;
        }

        /// <summary>
        /// 更新員工資料
        /// </summary>
        public bool UpdateEmployeeData(AccountParams param)
        {
            IDataAccessCommand cmd = DataAccessCommandFactory.GetDataAccessCommand(DBs.MainDB);
            spEmployee_UpdateData cmdInfo = new spEmployee_UpdateData()
            {
                EmpId = param.EmpId,
                EmpPassword = param.EmpPassword,
                EmpName = param.EmpName,
                Email = param.Email,
                Remarks = param.Remarks,
                DeptId = param.DeptId,
                RoleId = param.RoleId,
                IsAccessDenied = param.IsAccessDenied,
                StartDate = param.StartDate,
                EndDate = param.EndDate,
                OwnerAccount = param.OwnerAccount,
                PasswordHashed = param.PasswordHashed,
                DefaultRandomPassword = param.DefaultRandomPassword,
                MdfAccount = param.PostAccount
            };
            bool result = cmd.ExecuteNonQuery(cmdInfo);
            dbErrMsg = cmd.GetErrMsg();

            return result;
        }

        #endregion

        #region Operation DataAccess functions

        /// <summary>
        /// 取得後端作業選項第一層清單和身分授權
        /// </summary>
        public DataSet GetOperationsTopListWithRoleAuth(string roleName)
        {
            IDataAccessCommand cmd = DataAccessCommandFactory.GetDataAccessCommand(DBs.MainDB);
            spOperations_GetTopListWithRoleAuth cmdInfo = new spOperations_GetTopListWithRoleAuth()
            {
                RoleName = roleName
            };
            DataSet ds = cmd.ExecuteDataset(cmdInfo);
            dbErrMsg = cmd.GetErrMsg();

            return ds;
        }

        /// <summary>
        /// 取得後端作業選項子清單和身分授權
        /// </summary>
        public DataSet GetOperationsSubListWithRoleAuth(string roleName)
        {
            IDataAccessCommand cmd = DataAccessCommandFactory.GetDataAccessCommand(DBs.MainDB);
            spOperations_GetSubListWithRoleAuth cmdInfo = new spOperations_GetSubListWithRoleAuth()
            {
                RoleName = roleName
            };
            DataSet ds = cmd.ExecuteDataset(cmdInfo);
            dbErrMsg = cmd.GetErrMsg();

            return ds;
        }

        /// <summary>
        /// 取得後端作業選項資料
        /// </summary>
        public DataSet GetOperationData(int opId)
        {
            IDataAccessCommand cmd = DataAccessCommandFactory.GetDataAccessCommand(DBs.MainDB);
            spOperations_GetData cmdInfo = new spOperations_GetData()
            {
                OpId = opId
            };
            DataSet ds = cmd.ExecuteDataset(cmdInfo);
            dbErrMsg = cmd.GetErrMsg();

            return ds;
        }

        /// <summary>
        /// 取得用來組成麵包屑節點連結的後端作業選項資料
        /// </summary>
        public OperationHtmlAnchorData GetOperationHtmlAnchorData(int opId)
        {
            OperationHtmlAnchorData result = null;
            DataSet dsOp = GetOperationData(opId);

            if (dsOp != null && dsOp.Tables[0].Rows.Count > 0)
            {
                DataRow drFirst = dsOp.Tables[0].Rows[0];

                result = new OperationHtmlAnchorData();
                result.Subject = drFirst.ToSafeStr("OpSubject");
                result.LinkUrl = drFirst.ToSafeStr("LinkUrl");
                result.IconImageFileUrl = drFirst.ToSafeStr("IconImageFile");
                result.Html = string.Format("<a href=\"{0}\">{1}</a>", result.LinkUrl, result.Subject);
            }

            return result;
        }

        #endregion

        #region EmployeeRole DataAccess functions

        /// <summary>
        /// 取得選擇用員工身分清單
        /// </summary>
        public DataSet GetEmployeeRoleListToSelect()
        {
            IDataAccessCommand cmd = DataAccessCommandFactory.GetDataAccessCommand(DBs.MainDB);
            spEmployeeRole_GetListToSelect cmdInfo = new spEmployeeRole_GetListToSelect();

            DataSet ds = cmd.ExecuteDataset(cmdInfo);
            dbErrMsg = cmd.GetErrMsg();

            return ds;
        }

        /// <summary>
        /// 取得員工身分清單
        /// </summary>
        public DataSet GetEmployeeRoleList(RoleListQueryParams param)
        {
            IDataAccessCommand cmd = DataAccessCommandFactory.GetDataAccessCommand(DBs.MainDB);
            spEmployeeRole_GetList cmdInfo = new spEmployeeRole_GetList()
            {
                Kw = param.Kw,
                BeginNum = param.PagedParams.BeginNum,
                EndNum = param.PagedParams.EndNum,
                SortField = param.PagedParams.SortField,
                IsSortDesc = param.PagedParams.IsSortDesc
            };
            DataSet ds = cmd.ExecuteDataset(cmdInfo);
            dbErrMsg = cmd.GetErrMsg();
            param.PagedParams.RowCount = cmdInfo.RowCount;

            return ds;
        }

        /// <summary>
        /// 刪除員工身分
        /// </summary>
        public bool DeleteEmployeeRoleData(RoleParams param)
        {
            IDataAccessCommand cmd = DataAccessCommandFactory.GetDataAccessCommand(DBs.MainDB);
            spEmployeeRole_DeleteData cmdInfo = new spEmployeeRole_DeleteData()
            {
                RoleId = param.RoleId
            };
            bool result = cmd.ExecuteNonQuery(cmdInfo);
            dbErrMsg = cmd.GetErrMsg();

            if (!result && cmd.GetSqlErrNumber() == 50000 && cmd.GetSqlErrState() == 2)
            {
                param.IsThereAccountsOfRole = true;
            }

            return result;
        }

        /// <summary>
        /// 取得員工身分最大排序編號
        /// </summary>
        public int GetEmployeeRoleMaxSortNo()
        {
            IDataAccessCommand cmd = DataAccessCommandFactory.GetDataAccessCommand(DBs.MainDB);
            spEmployeeRole_GetMaxSortNo cmdInfo = new spEmployeeRole_GetMaxSortNo();

            int errCode = -1;
            int result = cmd.ExecuteScalar<int>(cmdInfo, errCode);
            dbErrMsg = cmd.GetErrMsg();

            return result;
        }

        /// <summary>
        /// 取得員工身分資料
        /// </summary>
        public DataSet GetEmployeeRoleData(int roleId)
        {
            IDataAccessCommand cmd = DataAccessCommandFactory.GetDataAccessCommand(DBs.MainDB);
            spEmployeeRole_GetData cmdInfo = new spEmployeeRole_GetData()
            {
                RoleId = roleId
            };
            DataSet ds = cmd.ExecuteDataset(cmdInfo);
            dbErrMsg = cmd.GetErrMsg();

            return ds;
        }

        /// <summary>
        /// 新增員工身分資料
        /// </summary>
        public bool InsertEmployeeRoleData(RoleParams param)
        {
            IDataAccessCommand cmd = DataAccessCommandFactory.GetDataAccessCommand(DBs.MainDB);
            spEmployeeRole_InsertData cmdInfo = new spEmployeeRole_InsertData()
            {
                RoleName = param.RoleName,
                RoleDisplayName = param.RoleDisplayName,
                SortNo = param.SortNo,
                CopyPrivilegeFromRoleName = param.CopyPrivilegeFromRoleName,
                PostAccount = param.PostAccount
            };
            bool result = cmd.ExecuteNonQuery(cmdInfo);
            dbErrMsg = cmd.GetErrMsg();

            if (result)
            {
                param.RoleId = cmdInfo.RoleId;
            }
            else if (cmd.GetSqlErrNumber() == 50000 && cmd.GetSqlErrState() == 2)
            {
                param.HasRoleBeenUsed = true;
            }

            return result;
        }

        /// <summary>
        /// 更新員工身分資料
        /// </summary>
        public bool UpdateEmployeeRoleData(RoleParams param)
        {
            IDataAccessCommand cmd = DataAccessCommandFactory.GetDataAccessCommand(DBs.MainDB);
            spEmployeeRole_UpdateData cmdInfo = new spEmployeeRole_UpdateData()
            {
                RoleId = param.RoleId,
                RoleDisplayName = param.RoleDisplayName,
                SortNo = param.SortNo,
                MdfAccount = param.PostAccount
            };
            bool result = cmd.ExecuteNonQuery(cmdInfo);
            dbErrMsg = cmd.GetErrMsg();

            return result;
        }

        #endregion

        #region EmployeeRole-Operations DataAccess functions

        /// <summary>
        /// 儲存員工身分後端作業授權清單
        /// </summary>
        public bool SaveListOfEmployeeRolePrivileges(RolePrivilegeParams param)
        {
            IDataAccessCommand cmd = DataAccessCommandFactory.GetDataAccessCommand(DBs.MainDB);
            SaveListOfEmployeeRoleOperationsDesc cmdInfo = new SaveListOfEmployeeRoleOperationsDesc()
            {
                RoleName = param.RoleName,
                roleOps = param.GetRoleOpsOfDA(),
                PostAccount = param.PostAccount
            };
            bool result = cmd.ExecuteNonQuery(cmdInfo);
            dbErrMsg = cmd.GetErrMsg();

            return result;
        }

        #endregion

        #region Department DataAccess functions

        /// <summary>
        /// 取得選擇用部門清單
        /// </summary>
        public DataSet GetDepartmentListToSelect()
        {
            IDataAccessCommand cmd = DataAccessCommandFactory.GetDataAccessCommand(DBs.MainDB);
            spDepartment_GetListToSelect cmdInfo = new spDepartment_GetListToSelect();

            DataSet ds = cmd.ExecuteDataset(cmdInfo);
            dbErrMsg = cmd.GetErrMsg();

            return ds;
        }

        #endregion
    }
}
