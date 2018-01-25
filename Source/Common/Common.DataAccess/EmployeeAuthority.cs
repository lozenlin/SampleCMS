// ===============================================================================
// DataAccessCommandInfo of EmployeeAuthority of SampleCMS
// https://github.com/lozenlin/SampleCMS
//
// EmployeeAuthority.cs
//
// ===============================================================================
// Copyright (c) 2017 lozenlin
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// ===============================================================================

using log4net;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Common.DataAccess.EmployeeAuthority
{
    #region 員工資料

    /// <summary>
    /// 取得員工登入用資料
    /// </summary>
    public class spEmployee_GetDataToLogin : IDataAccessCommandInfo
    {
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value) from these fields automatically. Property is not included.
        // Output parameter needs attribute [OutputPara]
        public string EmpAccount;

        public CommandType GetCommandType()
        {
            return CommandType.StoredProcedure;
        }

        public string GetCommandText()
        {
            return "dbo.spEmployee_GetDataToLogin";
        }
    }

    /// <summary>
    /// 取得員工資料
    /// </summary>
    public class spEmployee_GetData : IDataAccessCommandInfo
    {
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value) from these fields automatically. Property is not included.
        // Output parameter needs attribute [OutputPara]
        public string EmpAccount;

        public CommandType GetCommandType()
        {
            return CommandType.StoredProcedure;
        }

        public string GetCommandText()
        {
            return "dbo.spEmployee_GetData";
        }
    }

    /// <summary>
    /// 取得員工代碼的帳號
    /// </summary>
    public class spEmployee_GetAccountOfId : IDataAccessCommandInfo
    {
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value) from these fields automatically. Property is not included.
        // Output parameter needs attribute [OutputPara]
        public int EmpId;

        public CommandType GetCommandType()
        {
            return CommandType.StoredProcedure;
        }

        public string GetCommandText()
        {
            return "dbo.spEmployee_GetAccountOfId";
        }
    }

    /// <summary>
    /// 取得員工清單
    /// </summary>
    public class spEmployee_GetList : IDataAccessCommandInfo
    {
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value) from these fields automatically. Property is not included.
        // Output parameter needs attribute [OutputPara]
        public int DeptId;  //0:all
        public string Kw;
        public int ListMode;    //清單內容模式(0:all, 1:normal, 2:access is denied)
        public int BeginNum;
        public int EndNum;
        public string SortField;
        public bool IsSortDesc;
        public bool CanReadSubItemOfOthers;
        public bool CanReadSubItemOfCrew;
        public bool CanReadSubItemOfSelf;
        public string MyAccount;
        public int MyDeptId;
        [OutputPara]
        public int RowCount;

        public spEmployee_GetList()
        {
        }

        public CommandType GetCommandType()
        {
            return CommandType.StoredProcedure;
        }
        
        public string GetCommandText()
        {
            return "dbo.spEmployee_GetList";
        }
    }

    /// <summary>
    /// 取得員工身分名稱
    /// </summary>
    public class spEmployee_GetRoleName : IDataAccessCommandInfo
    {
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value) from these fields automatically. Property is not included.
        // Output parameter needs attribute [OutputPara]
        public string EmpAccount;

        public CommandType GetCommandType()
        {
            return CommandType.StoredProcedure;
        }

        public string GetCommandText()
        {
            return "dbo.spEmployee_GetRoleName";
        }
    }

    /// <summary>
    /// 更新員工本次登入資訊
    /// </summary>
    public class spEmployee_UpdateLoginInfo : IDataAccessCommandInfo
    {
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value) from these fields automatically. Property is not included.
        // Output parameter needs attribute [OutputPara]
        public string EmpAccount;
        public string ThisLoginIP;

        public CommandType GetCommandType()
        {
            return CommandType.StoredProcedure;
        }

        public string GetCommandText()
        {
            return "dbo.spEmployee_UpdateLoginInfo";
        }
    }

    /// <summary>
    /// 刪除員工資料
    /// </summary>
    public class spEmployee_DeleteData : IDataAccessCommandInfo
    {
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value) from these fields automatically. Property is not included.
        // Output parameter needs attribute [OutputPara]
        public int EmpId;

        public CommandType GetCommandType()
        {
            return CommandType.StoredProcedure;
        }

        public string GetCommandText()
        {
            return "dbo.spEmployee_DeleteData";
        }
    }

    /// <summary>
    /// 新增員工資料
    /// </summary>
    public class spEmployee_InsertData : IDataAccessCommandInfo
    {
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value) from these fields automatically. Property is not included.
        // Output parameter needs attribute [OutputPara]
        public string EmpAccount;
        public string EmpPassword;
        public string EmpName;
        public string Email;
        public string Remarks;
        public int DeptId;
        public int RoleId;
        public bool IsAccessDenied;
        public DateTime StartDate;
        public DateTime EndDate;
        public string OwnerAccount;
        public bool PasswordHashed;
        public string DefaultRandomPassword;
        public string PostAccount;
        [OutputPara]
        public int EmpId;

        public CommandType GetCommandType()
        {
            return CommandType.StoredProcedure;
        }

        public string GetCommandText()
        {
            return "dbo.spEmployee_InsertData";
        }
    }

    /// <summary>
    /// 更新員工資料
    /// </summary>
    public class spEmployee_UpdateData : IDataAccessCommandInfo
    {
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value) from these fields automatically. Property is not included.
        // Output parameter needs attribute [OutputPara]
        public int EmpId;
        public string EmpPassword;
        public string EmpName;
        public string Email;
        public string Remarks;
        public int DeptId;
        public int RoleId;
        public bool IsAccessDenied;
        public DateTime StartDate;
        public DateTime EndDate;
        public string OwnerAccount;
        public bool PasswordHashed;
        public string DefaultRandomPassword;
        public string MdfAccount;

        public CommandType GetCommandType()
        {
            return CommandType.StoredProcedure;
        }

        public string GetCommandText()
        {
            return "dbo.spEmployee_UpdateData";
        }
    }

    /// <summary>
    /// 更新員工密碼
    /// </summary>
    public class spEmployee_UpdatePassword : IDataAccessCommandInfo
    {
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value) from these fields automatically. Property is not included.
        // Output parameter needs attribute [OutputPara]
        public string EmpAccount;
        public string EmpPassword;

        public CommandType GetCommandType()
        {
            return CommandType.StoredProcedure;
        }

        public string GetCommandText()
        {
            return "dbo.spEmployee_UpdatePassword";
        }
    }

    /// <summary>
    /// 更新員工的重置密碼用唯一值
    /// </summary>
    public class spEmployee_UpdatePasswordResetKey : IDataAccessCommandInfo
    {
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value) from these fields automatically. Property is not included.
        // Output parameter needs attribute [OutputPara]
        public string EmpAccount;
        public string PasswordResetKey;

        public CommandType GetCommandType()
        {
            return CommandType.StoredProcedure;
        }

        public string GetCommandText()
        {
            return "dbo.spEmployee_UpdatePasswordResetKey";
        }
    }

    /// <summary>
    /// 以重置密碼用唯一值取得員工登入用資料
    /// </summary>
    public class spEmployee_GetDataToLoginByPasswordResetKey : IDataAccessCommandInfo
    {
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value) from these fields automatically. Property is not included.
        // Output parameter needs attribute [OutputPara]
        public string PasswordResetKey;

        public CommandType GetCommandType()
        {
            return CommandType.StoredProcedure;
        }

        public string GetCommandText()
        {
            return "dbo.spEmployee_GetDataToLoginByPasswordResetKey";
        }
    }

    #endregion

    #region 後端操作記錄

    /// <summary>
    /// 新增後端操作記錄
    /// </summary>
    public class spBackEndLog_InsertData : IDataAccessCommandInfo
    {
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value) from these fields automatically. Property is not included.
        // Output parameter needs attribute [OutputPara]
        public string EmpAccount;
        public string Description;
        public string IP;

        public CommandType GetCommandType()
        {
            return CommandType.StoredProcedure;
        }

        public string GetCommandText()
        {
            return "dbo.spBackEndLog_InsertData";
        }
    }

    /// <summary>
    /// 取得後端操作記錄清單
    /// </summary>
    public class spBackEndLog_GetList : IDataAccessCommandInfo
    {
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value) from these fields automatically. Property is not included.
        // Output parameter needs attribute [OutputPara]
        public DateTime StartDate;
        public DateTime EndDate;
        public string Account;  // 空字串:全部
        public bool IsAccKw;
        public string IP;   // 空字串:全部
        public bool IsIpHeadKw;
        public string DescKw;   // 空字串:全部
        public int RangeMode;   // 0:全部, 1:登入相關
        public int BeginNum;
        public int EndNum;
        public string SortField;
        public bool IsSortDesc;
        public bool CanReadSubItemOfOthers;
        public bool CanReadSubItemOfCrew;
        public bool CanReadSubItemOfSelf;
        public string MyAccount;
        public int MyDeptId;
        [OutputPara]
        public int RowCount;

        public CommandType GetCommandType()
        {
            return CommandType.StoredProcedure;
        }

        public string GetCommandText()
        {
            return "dbo.spBackEndLog_GetList";
        }
    }

    #endregion

    #region 網頁後端作業選項相關

    /// <summary>
    /// 用共用元件類別名稱取得後端作業選項資訊
    /// </summary>
    public class spOperations_GetOpInfoByCommonClass : IDataAccessCommandInfo
    {
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value) from these fields automatically. Property is not included.
        // Output parameter needs attribute [OutputPara]
        public string CommonClass;

        public CommandType GetCommandType()
        {
            return CommandType.StoredProcedure;
        }

        public string GetCommandText()
        {
            return "dbo.spOperations_GetOpInfoByCommonClass";
        }
    }

    /// <summary>
    /// 用超連結網址取得後端作業選項資訊
    /// </summary>
    public class spOperations_GetOpInfoByLinkUrl : IDataAccessCommandInfo
    {
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value) from these fields automatically. Property is not included.
        // Output parameter needs attribute [OutputPara]
        public string LinkUrl;

        public CommandType GetCommandType()
        {
            return CommandType.StoredProcedure;
        }

        public string GetCommandText()
        {
            return "dbo.spOperations_GetOpInfoByLinkUrl";
        }
    }

    /// <summary>
    /// 取得後端作業選項第一層清單和身分授權
    /// </summary>
    public class spOperations_GetTopListWithRoleAuth : IDataAccessCommandInfo
    {
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value) from these fields automatically. Property is not included.
        // Output parameter needs attribute [OutputPara]
        public string RoleName;

        public CommandType GetCommandType()
        {
            return CommandType.StoredProcedure;
        }

        public string GetCommandText()
        {
            return "dbo.spOperations_GetTopListWithRoleAuth";
        }
    }

    /// <summary>
    /// 取得後端作業選項子清單和身分授權
    /// </summary>
    public class spOperations_GetSubListWithRoleAuth : IDataAccessCommandInfo
    {
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value) from these fields automatically. Property is not included.
        // Output parameter needs attribute [OutputPara]
        public string RoleName;

        public CommandType GetCommandType()
        {
            return CommandType.StoredProcedure;
        }

        public string GetCommandText()
        {
            return "dbo.spOperations_GetSubListWithRoleAuth";
        }
    }

    /// <summary>
    /// 取得後端作業選項資料
    /// </summary>
    public class spOperations_GetData : IDataAccessCommandInfo
    {
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value) from these fields automatically. Property is not included.
        // Output parameter needs attribute [OutputPara]
        public int OpId;

        public CommandType GetCommandType()
        {
            return CommandType.StoredProcedure;
        }

        public string GetCommandText()
        {
            return "dbo.spOperations_GetData";
        }
    }

    /// <summary>
    /// 取得後端作業選項清單
    /// </summary>
    public class spOperations_GetList : IDataAccessCommandInfo
    {
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value) from these fields automatically. Property is not included.
        // Output parameter needs attribute [OutputPara]
        public int ParentId;	// 0:root
        public string CultureName;
        public string Kw;
        public int BeginNum;
        public int EndNum;
        public string SortField;
        public bool IsSortDesc;
        public bool CanReadSubItemOfOthers;
        public bool CanReadSubItemOfCrew;
        public bool CanReadSubItemOfSelf;
        public string MyAccount;
        public int MyDeptId;
        [OutputPara]
        public int RowCount;

        public CommandType GetCommandType()
        {
            return CommandType.StoredProcedure;
        }

        public string GetCommandText()
        {
            return "dbo.spOperations_GetList";
        }
    }

    /// <summary>
    /// 刪除後端作業選項
    /// </summary>
    public class spOperations_DeleteData : IDataAccessCommandInfo
    {
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value) from these fields automatically. Property is not included.
        // Output parameter needs attribute [OutputPara]
        public int OpId;

        public CommandType GetCommandType()
        {
            return CommandType.StoredProcedure;
        }

        public string GetCommandText()
        {
            return "dbo.spOperations_DeleteData";
        }
    }

    /// <summary>
    /// 加大後端作業選項的排序編號
    /// </summary>
    public class spOperations_IncreaseSortNo : IDataAccessCommandInfo
    {
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value) from these fields automatically. Property is not included.
        // Output parameter needs attribute [OutputPara]
        public int OpId;
        public string MdfAccount;

        public CommandType GetCommandType()
        {
            return CommandType.StoredProcedure;
        }

        public string GetCommandText()
        {
            return "dbo.spOperations_IncreaseSortNo";
        }
    }

    /// <summary>
    /// 減小後端作業選項的排序編號
    /// </summary>
    public class spOperations_DecreaseSortNo : IDataAccessCommandInfo
    {
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value) from these fields automatically. Property is not included.
        // Output parameter needs attribute [OutputPara]
        public int OpId;
        public string MdfAccount;

        public CommandType GetCommandType()
        {
            return CommandType.StoredProcedure;
        }

        public string GetCommandText()
        {
            return "dbo.spOperations_DecreaseSortNo";
        }
    }

    /// <summary>
    /// 取得後端作業選項階層資訊
    /// </summary>
    public class spOperations_GetLevelInfo : IDataAccessCommandInfo
    {
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value) from these fields automatically. Property is not included.
        // Output parameter needs attribute [OutputPara]
        public int OpId;

        public CommandType GetCommandType()
        {
            return CommandType.StoredProcedure;
        }

        public string GetCommandText()
        {
            return "dbo.spOperations_GetLevelInfo";
        }
    }

    /// <summary>
    /// 取得後端作業選項最大排序編號
    /// </summary>
    public class spOperations_GetMaxSortNo : IDataAccessCommandInfo
    {
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value) from these fields automatically. Property is not included.
        // Output parameter needs attribute [OutputPara]
        public int ParentId;    // 0:root

        public CommandType GetCommandType()
        {
            return CommandType.StoredProcedure;
        }

        public string GetCommandText()
        {
            return "dbo.spOperations_GetMaxSortNo";
        }
    }

    /// <summary>
    /// 新增後端作業選項
    /// </summary>
    public class spOperations_InsertData : IDataAccessCommandInfo
    {
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value) from these fields automatically. Property is not included.
        // Output parameter needs attribute [OutputPara]
        public int ParentId;	// 0: root
        public string OpSubject;
        public string LinkUrl;
        public bool IsNewWindow;
        public string IconImageFile;
        public int SortNo;
        public bool IsHideSelf;
        public string CommonClass;
        public string PostAccount;
        public string EnglishSubject;
        [OutputPara]
        public int OpId;

        public CommandType GetCommandType()
        {
            return CommandType.StoredProcedure;
        }

        public string GetCommandText()
        {
            return "dbo.spOperations_InsertData";
        }
    }

    /// <summary>
    /// 更新後端作業選項
    /// </summary>
    public class spOperations_UpdateData : IDataAccessCommandInfo
    {
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value) from these fields automatically. Property is not included.
        // Output parameter needs attribute [OutputPara]
        public int OpId;
        public string OpSubject;
        public string LinkUrl;
        public bool IsNewWindow;
        public string IconImageFile;
        public int SortNo;
        public bool IsHideSelf;
        public string CommonClass;
        public string MdfAccount;
        public string EnglishSubject;

        public CommandType GetCommandType()
        {
            return CommandType.StoredProcedure;
        }

        public string GetCommandText()
        {
            return "dbo.spOperations_UpdateData";
        }
    }

    #endregion

    #region 員工身分後端作業授權相關

    /// <summary>
    /// 取得指定作業代碼的後端身分可使用權限
    /// </summary>
    public class spEmployeeRoleOperationsDesc_GetDataOfOp : IDataAccessCommandInfo
    {
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value) from these fields automatically. Property is not included.
        // Output parameter needs attribute [OutputPara]
        public string RoleName;
        public int OpId;

        public CommandType GetCommandType()
        {
            return CommandType.StoredProcedure;
        }

        public string GetCommandText()
        {
            return "dbo.spEmployeeRoleOperationsDesc_GetDataOfOp";
        }
    }

    /// <summary>
    /// 儲存員工身分後端作業授權
    /// </summary>
    public class spEmployeeRoleOperationsDesc_SaveData : IDataAccessCommandInfo
    {
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value) from these fields automatically. Property is not included.
        // Output parameter needs attribute [OutputPara]
        public string RoleName;
        public int OpId;
        public bool CanRead;
        public bool CanEdit;
        public bool CanReadSubItemOfSelf;
        public bool CanEditSubItemOfSelf;
        public bool CanAddSubItemOfSelf;
        public bool CanDelSubItemOfSelf;
        public bool CanReadSubItemOfCrew;
        public bool CanEditSubItemOfCrew;
        public bool CanDelSubItemOfCrew;
        public bool CanReadSubItemOfOthers;
        public bool CanEditSubItemOfOthers;
        public bool CanDelSubItemOfOthers;
        public string PostAccount;

        public CommandType GetCommandType()
        {
            return CommandType.StoredProcedure;
        }

        public string GetCommandText()
        {
            return "dbo.spEmployeeRoleOperationsDesc_SaveData";
        }
    }

    /// <summary>
    /// 儲存員工身分後端作業授權清單
    /// </summary>
    public class SaveListOfEmployeeRoleOperationsDesc : IDataAccessCommandInfo, ICustomExecuteNonQuery
    {
        public string RoleName;
        public List<RoleOpDescParamsDA> roleOps;
        public string PostAccount;

        public SaveListOfEmployeeRoleOperationsDesc()
        {
            roleOps = new List<RoleOpDescParamsDA>();
        }

        public CommandType GetCommandType()
        {
            return CommandType.StoredProcedure;
        }

        public string GetCommandText()
        {
            return "SaveListOfEmployeeRoleOperationsDesc";
        }

        public bool ExecuteNonQuery(IDataAccessCommandInnerTools innerTools)
        {
            if (roleOps.Count == 0)
            {
                innerTools.SetErrMsg("roleOps is empty");
                return false;
            }

            ILog Logger = innerTools.GetLogger();
            IDataAccessSource db = innerTools.GetDataAccessSource();
            SqlConnection conn = null;
            SqlTransaction tran = null;

            try
            {
                //建立連線資訊並開啟連線
                conn = db.CreateConnectionInstanceWithOpen();
                tran = conn.BeginTransaction();

                foreach (RoleOpDescParamsDA roleOp in roleOps)
                {
                    innerTools.SetLogSql("spEmployeeRoleOperationsDesc_SaveData",
                        RoleName,
                        roleOp.OpId,
                        roleOp.CanRead,
                        roleOp.CanEdit,
                        roleOp.CanReadSubItemOfSelf,
                        roleOp.CanEditSubItemOfSelf,
                        roleOp.CanAddSubItemOfSelf,
                        roleOp.CanDelSubItemOfSelf,
                        roleOp.CanReadSubItemOfCrew,
                        roleOp.CanEditSubItemOfCrew,
                        roleOp.CanDelSubItemOfCrew,
                        roleOp.CanReadSubItemOfOthers,
                        roleOp.CanEditSubItemOfOthers,
                        roleOp.CanDelSubItemOfOthers,
                        PostAccount
                        );

                    SqlHelper.ExecuteNonQuery(tran, "dbo.spEmployeeRoleOperationsDesc_SaveData",
                        RoleName,
                        roleOp.OpId,
                        roleOp.CanRead,
                        roleOp.CanEdit,
                        roleOp.CanReadSubItemOfSelf,
                        roleOp.CanEditSubItemOfSelf,
                        roleOp.CanAddSubItemOfSelf,
                        roleOp.CanDelSubItemOfSelf,
                        roleOp.CanReadSubItemOfCrew,
                        roleOp.CanEditSubItemOfCrew,
                        roleOp.CanDelSubItemOfCrew,
                        roleOp.CanReadSubItemOfOthers,
                        roleOp.CanEditSubItemOfOthers,
                        roleOp.CanDelSubItemOfOthers,
                        PostAccount
                        );
                }

                tran.Commit();
            }
            catch (Exception ex)
            {
                Logger.Error("", ex);

                //回傳錯誤訊息
                innerTools.SetErrMsg(ex.Message);

                if (tran != null)
                    tran.Rollback();

                return false;
            }
            finally
            {
                //關閉連線資訊
                db.CloseConnection(conn);
            }

            return true;
        }
    }

    #endregion

    #region 員工身分

    /// <summary>
    /// 取得選擇用員工身分清單
    /// </summary>
    public class spEmployeeRole_GetListToSelect : IDataAccessCommandInfo
    {
        public CommandType GetCommandType()
        {
            return CommandType.StoredProcedure;
        }

        public string GetCommandText()
        {
            return "dbo.spEmployeeRole_GetListToSelect";
        }
    }

    /// <summary>
    /// 取得員工身分清單
    /// </summary>
    public class spEmployeeRole_GetList : IDataAccessCommandInfo
    {
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value) from these fields automatically. Property is not included.
        // Output parameter needs attribute [OutputPara]
        public string Kw;
        public int BeginNum;
        public int EndNum;
        public string SortField;
        public bool IsSortDesc;
        public bool CanReadSubItemOfOthers;
        public bool CanReadSubItemOfCrew;
        public bool CanReadSubItemOfSelf;
        public string MyAccount;
        public int MyDeptId;
        [OutputPara]
        public int RowCount;

        public CommandType GetCommandType()
        {
            return CommandType.StoredProcedure;
        }

        public string GetCommandText()
        {
            return "dbo.spEmployeeRole_GetList";
        }
    }

    /// <summary>
    /// 刪除員工身分
    /// </summary>
    public class spEmployeeRole_DeleteData : IDataAccessCommandInfo
    {
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value) from these fields automatically. Property is not included.
        // Output parameter needs attribute [OutputPara]
        public int RoleId;

        public CommandType GetCommandType()
        {
            return CommandType.StoredProcedure;
        }

        public string GetCommandText()
        {
            return "dbo.spEmployeeRole_DeleteData";
        }
    }

    /// <summary>
    /// 取得員工身分最大排序編號
    /// </summary>
    public class spEmployeeRole_GetMaxSortNo : IDataAccessCommandInfo
    {
        public CommandType GetCommandType()
        {
            return CommandType.StoredProcedure;
        }

        public string GetCommandText()
        {
            return "dbo.spEmployeeRole_GetMaxSortNo";
        }
    }

    /// <summary>
    /// 取得員工身分資料
    /// </summary>
    public class spEmployeeRole_GetData : IDataAccessCommandInfo
    {
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value) from these fields automatically. Property is not included.
        // Output parameter needs attribute [OutputPara]
        public int RoleId;

        public CommandType GetCommandType()
        {
            return CommandType.StoredProcedure;
        }

        public string GetCommandText()
        {
            return "dbo.spEmployeeRole_GetData";
        }
    }


    /// <summary>
    /// 新增員工身分資料
    /// </summary>
    public class spEmployeeRole_InsertData : IDataAccessCommandInfo
    {
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value) from these fields automatically. Property is not included.
        // Output parameter needs attribute [OutputPara]
        public string RoleName;
        public string RoleDisplayName;
        public int SortNo;
        public string PostAccount;
        public string CopyPrivilegeFromRoleName;
        [OutputPara]
        public int RoleId;

        public CommandType GetCommandType()
        {
            return CommandType.StoredProcedure;
        }

        public string GetCommandText()
        {
            return "dbo.spEmployeeRole_InsertData";
        }
    }

    /// <summary>
    /// 更新員工身分資料
    /// </summary>
    public class spEmployeeRole_UpdateData : IDataAccessCommandInfo
    {
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value) from these fields automatically. Property is not included.
        // Output parameter needs attribute [OutputPara]
        public int RoleId;
        public string RoleDisplayName;
        public int SortNo;
        public string MdfAccount;

        public CommandType GetCommandType()
        {
            return CommandType.StoredProcedure;
        }

        public string GetCommandText()
        {
            return "dbo.spEmployeeRole_UpdateData";
        }
    }

    #endregion

    #region 部門資料

    /// <summary>
    /// 取得選擇用部門清單
    /// </summary>
    public class spDepartment_GetListToSelect : IDataAccessCommandInfo
    {
        public CommandType GetCommandType()
        {
            return CommandType.StoredProcedure;
        }

        public string GetCommandText()
        {
            return "dbo.spDepartment_GetListToSelect";
        }
    }

    /// <summary>
    /// 取得部門清單
    /// </summary>
    public class spDepartment_GetList : IDataAccessCommandInfo
    {
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value) from these fields automatically. Property is not included.
        // Output parameter needs attribute [OutputPara]
        public string Kw;
        public int BeginNum;
        public int EndNum;
        public string SortField;
        public bool IsSortDesc;
        public bool CanReadSubItemOfOthers;
        public bool CanReadSubItemOfCrew;
        public bool CanReadSubItemOfSelf;
        public string MyAccount;
        public int MyDeptId;
        [OutputPara]
        public int RowCount;

        public CommandType GetCommandType()
        {
            return CommandType.StoredProcedure;
        }

        public string GetCommandText()
        {
            return "dbo.spDepartment_GetList";
        }
    }

    /// <summary>
    /// 刪除部門資料
    /// </summary>
    public class spDepartment_DeleteData : IDataAccessCommandInfo
    {
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value) from these fields automatically. Property is not included.
        // Output parameter needs attribute [OutputPara]
        public int DeptId;

        public CommandType GetCommandType()
        {
            return CommandType.StoredProcedure;
        }

        public string GetCommandText()
        {
            return "dbo.spDepartment_DeleteData";
        }
    }

    /// <summary>
    /// 取得部門資料
    /// </summary>
    public class spDepartment_GetData : IDataAccessCommandInfo
    {
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value) from these fields automatically. Property is not included.
        // Output parameter needs attribute [OutputPara]
        public int DeptId;

        public CommandType GetCommandType()
        {
            return CommandType.StoredProcedure;
        }

        public string GetCommandText()
        {
            return "dbo.spDepartment_GetData";
        }
    }

    /// <summary>
    /// 取得部門最大排序編號
    /// </summary>
    public class spDepartment_GetMaxSortNo : IDataAccessCommandInfo
    {
        public CommandType GetCommandType()
        {
            return CommandType.StoredProcedure;
        }

        public string GetCommandText()
        {
            return "dbo.spDepartment_GetMaxSortNo";
        }
    }

    /// <summary>
    /// 新增部門資料
    /// </summary>
    public class spDepartment_InsertData : IDataAccessCommandInfo
    {
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value) from these fields automatically. Property is not included.
        // Output parameter needs attribute [OutputPara]
        public string DeptName;
        public int SortNo;
        public string PostAccount;
        [OutputPara]
        public int DeptId;

        public CommandType GetCommandType()
        {
            return CommandType.StoredProcedure;
        }

        public string GetCommandText()
        {
            return "dbo.spDepartment_InsertData";
        }
    }

    /// <summary>
    /// 更新部門資料
    /// </summary>
    public class spDepartment_UpdateData : IDataAccessCommandInfo
    {
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value) from these fields automatically. Property is not included.
        // Output parameter needs attribute [OutputPara]
        public int DeptId;
        public string DeptName;
        public int SortNo;
        public string MdfAccount;

        public CommandType GetCommandType()
        {
            return CommandType.StoredProcedure;
        }

        public string GetCommandText()
        {
            return "dbo.spDepartment_UpdateData";
        }
    }

    #endregion
}
