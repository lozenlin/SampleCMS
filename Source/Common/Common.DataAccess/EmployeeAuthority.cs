﻿using System;
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
            return "spEmployee_GetDataToLogin";
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
        public int pa1;
        public string pa2;

        public CommandType GetCommandType()
        {
            return CommandType.StoredProcedure;
        }

        public string GetCommandText()
        {
            return "spEmployee_GetData";
        }
    }

    /// <summary>
    /// 取得後端使用者清單(test)
    /// </summary>
    public class spEmployee_GetList : IDataAccessCommandInfo
    {
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值、順序)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value, order) from these fields automatically. Property is not be included.
        // Output parameter needs attribute [OutputPara]
        public int DeptId;
        public string SearchName;
        public int ListMode;
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
            return "spEmployee_GetList";
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
            return "spBackEndLog_InsertData";
        }
    }

    #endregion
}
