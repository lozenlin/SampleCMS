using log4net;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;

namespace Common.DataAccess.EmployeeAuthorityDataAccess
{

    /// <summary>
    /// 取得後端使用者清單(test)
    /// </summary>
    public class spEmployee_GetList : DataAccessCommandBase
    {
        public int DeptId;
        public string SearchName;
        public int ListMode;
        public int BeginNum;
        public int EndNum;
        public string SortField;
        public bool IsSortDesc;
        [OutputPara]
        public int RowCount;

        public spEmployee_GetList(DataAccessSource dataAccessSource)
            : base(dataAccessSource)
        {
            cmdType = CommandType.StoredProcedure;
            cmdText = "spEmployee_GetList";
        }

        protected override void ModifyParaInfosBeforeExecute(List<SqlParaInfo> paraInfos)
        {
            foreach (SqlParaInfo paraInfo in paraInfos)
            {
                switch (paraInfo.Name)
                {
                    case "RowCount":
                        paraInfo.ParaFieldInfo.SetValue(this, -1);
                        break;
                }
            }
        }
    }

    /// <summary>
    /// 取得後端使用者登入用資料
    /// </summary>
    public class spEmployee_GetDataToLogin : DataAccessCommandBase
    {
        public string EmpAccount;
        public int defValue = 123;
        public bool IsTest = false;
        public DateTime StartDate;
        [OutputPara()]
        public int newId;

        public spEmployee_GetDataToLogin(DataAccessSource dataAccessSource)
            : base(dataAccessSource)
        {
            cmdType = CommandType.StoredProcedure;
            cmdText = "spEmployee_GetDataToLogin";
        }
    }
}
