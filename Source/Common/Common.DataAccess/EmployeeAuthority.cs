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

using System;
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
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值、順序)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value, order) from these fields automatically. Property is not be included.
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
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值、順序)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value, order) from these fields automatically. Property is not be included.
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
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值、順序)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value, order) from these fields automatically. Property is not be included.
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
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值、順序)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value, order) from these fields automatically. Property is not be included.
        // Output parameter needs attribute [OutputPara]
        public int DeptId;  //0:all
        public string Kw;
        public int ListMode;    //清單內容模式(0:all, 1:normal, 2:access denied)
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
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值、順序)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value, order) from these fields automatically. Property is not be included.
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
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值、順序)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value, order) from these fields automatically. Property is not be included.
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
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值、順序)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value, order) from these fields automatically. Property is not be included.
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
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值、順序)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value, order) from these fields automatically. Property is not be included.
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
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值、順序)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value, order) from these fields automatically. Property is not be included.
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

    #endregion

    #region 後端操作記錄

    /// <summary>
    /// 新增後端操作記錄
    /// </summary>
    public class spBackEndLog_InsertData : IDataAccessCommandInfo
    {
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值、順序)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value, order) from these fields automatically. Property is not be included.
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

    #endregion

    #region 網頁後端作業選項相關

    /// <summary>
    /// 用共用元件類別名稱取得後端作業選項資訊
    /// </summary>
    public class spOperations_GetOpInfoByCommonClass : IDataAccessCommandInfo
    {
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值、順序)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value, order) from these fields automatically. Property is not be included.
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
    /// 取得後端作業選項第一層清單和身分授權
    /// </summary>
    public class spOperations_GetTopListWithRoleAuth : IDataAccessCommandInfo
    {
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值、順序)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value, order) from these fields automatically. Property is not be included.
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
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值、順序)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value, order) from these fields automatically. Property is not be included.
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
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值、順序)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value, order) from these fields automatically. Property is not be included.
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

    #endregion

    #region 員工身分後端作業授權相關

    /// <summary>
    /// 取得指定作業代碼的後端身分可使用權限
    /// </summary>
    public class spEmployeeRoleOperationsDesc_GetDataOfOp : IDataAccessCommandInfo
    {
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值、順序)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value, order) from these fields automatically. Property is not be included.
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
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值、順序)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value, order) from these fields automatically. Property is not be included.
        // Output parameter needs attribute [OutputPara]
        public string Kw;
        public int BeginNum;
        public int EndNum;
        public string SortField;
        public bool IsSortDesc;
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
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值、順序)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value, order) from these fields automatically. Property is not be included.
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
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值、順序)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value, order) from these fields automatically. Property is not be included.
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
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值、順序)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value, order) from these fields automatically. Property is not be included.
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
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值、順序)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value, order) from these fields automatically. Property is not be included.
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

    #endregion

    #region emptyTail
    #endregion
}
