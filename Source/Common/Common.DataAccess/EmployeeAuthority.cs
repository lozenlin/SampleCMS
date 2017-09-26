using System;
using System.Data;
using System.Data.SqlClient;

namespace Common.DataAccess.EmployeeAuthority
{

    /// <summary>
    /// 取得後端使用者清單(test)
    /// </summary>
    public class spEmployee_GetList : IDataAccessCommandInfo, IModifyCommandParametersBeforeExecute
    {
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值、順序)；屬性不包含在其中。
        // DataAccessCommand generates SqlParameter information(name, value, order) from these fields automatically. Property is not be included.
        // 輸出參數請加上屬性 [OutputPara]
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

        public void ModifyCommandParametersBeforeExecute(SqlParameter[] commandParameters)
        {
            foreach (SqlParameter pa in commandParameters)
            {
                switch (pa.ParameterName)
                {
                    case "@RowCount":
                        pa.Value = -1;
                        break;
                }
            }
        }
    }

    /// <summary>
    /// 取得後端使用者登入用資料
    /// </summary>
    public class spEmployee_GetDataToLogin : IDataAccessCommandInfo
    {
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值、順序)；屬性不包含在其中。
        // DataAccessCommand generates SqlParameter information(name, value, order) from these fields automatically. Property is not be included.
        // 輸出參數請加上屬性 [OutputPara]
        // Output parameter needs attribute [OutputPara]
        public string EmpAccount;
        public int defValue = 123;
        public bool IsTest = false;
        public DateTime StartDate;
        [OutputPara()]
        public int newId;

        public spEmployee_GetDataToLogin()
        {
        }

        public CommandType GetCommandType()
        {
            return CommandType.StoredProcedure;
        }

        public string GetCommandText()
        {
            return "spEmployee_GetDataToLogin";
        }
    }
}
