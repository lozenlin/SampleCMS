using System;
using System.Data;
using System.Data.SqlClient;

namespace Common.DataAccess.EmployeeAuthority
{
    #region 後端使用者相關

    /// <summary>
    /// 取得後端使用者登入用資料
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
    /// 取得後端使用者資料
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
    /// 取得後端使用者清單
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
    /// 取得後端使用者角色名稱
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
    /// 更新後端使用者本次登入資訊
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
    /// 刪除後端使用者資料
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
    /// 取得後端作業選項第一層清單和角色授權
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
    /// 取得後端作業選項子清單和角色授權
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

    #region 員工角色後端作業授權相關

    /// <summary>
    /// 取得指定作業代碼的後端角色可使用權限
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

    #region 員工角色

    /// <summary>
    /// 取得選擇用員工角色清單
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
