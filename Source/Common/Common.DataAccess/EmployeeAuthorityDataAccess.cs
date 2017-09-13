using log4net;
using Microsoft.ApplicationBlocks.Data;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;

namespace Common.DataAccess.EmployeeAuthorityDataAccess
{
    /// <summary>
    /// 基礎資料庫存取指令功能
    /// </summary>
    public abstract class DataAccessCommandBase
    {
        /// <summary>
        /// 記錄工具
        /// </summary>
        public ILog Logger
        {
            get;
            protected set;
        }

        /// <summary>
        /// 執行後的錯誤訊息
        /// </summary>
        public string ErrMsg
        {
            get;
            protected set;
        }

        protected DataAccessSource db = null;
        protected CommandType cmdType = CommandType.Text;
        protected string cmdText = "";

        private SqlConnection conn = null;

        public DataAccessCommandBase(DataAccessSource dataAccessSource)
        {
            db = dataAccessSource;
            Logger = LogManager.GetLogger(this.GetType());
        }

        #region LogSql

        /// <summary>
        /// 記錄送去資料庫的指令和參數值
        /// </summary>
        public void LogSql(string commandText, params object[] parameterValues)
        {
            if (Logger.IsDebugEnabled)
            {
                try
                {
                    string separator = ", ";
                    StringBuilder sbParams = new StringBuilder(200);
                    foreach (object tempValue in parameterValues)
                    {
                        if (tempValue == null)
                            sbParams.Append("(null)");
                        else
                            sbParams.Append(tempValue.ToString());

                        sbParams.Append(separator);
                    }

                    int separatorLen = separator.Length;
                    if (sbParams.Length >= separatorLen)
                    {
                        sbParams.Remove(sbParams.Length - separatorLen, separatorLen);
                    }

                    Logger.DebugFormat("sql: {0}; params: {1};", commandText, sbParams.ToString());
                }
                catch (Exception ex)
                {
                    Logger.Error("params object", ex);
                }
            }
        }

        /// <summary>
        /// 記錄送去資料庫的指令和參數值
        /// </summary>
        public void LogSql(string commandText, params SqlParameter[] commandParameters)
        {
            if (Logger.IsDebugEnabled)
            {
                try
                {
                    string separator = ", ";
                    StringBuilder sbParams = new StringBuilder(500);
                    foreach (SqlParameter tempParam in commandParameters)
                    {
                        string tempValue = null;
                        if (tempParam.Value == null)
                        {
                            tempValue = "(null)";
                        }
                        else if (Convert.IsDBNull(tempParam.Value))
                        {
                            tempValue = "(DBNull)";
                        }
                        else
                        {
                            tempValue = tempParam.Value.ToString();
                        }

                        switch (tempParam.Direction)
                        {
                            case ParameterDirection.Output:
                            case ParameterDirection.InputOutput:
                                sbParams.Append("output ");
                                break;
                        }

                        sbParams.AppendFormat("{0}={1}", tempParam.ParameterName, tempValue);
                        sbParams.Append(separator);
                    }

                    int separatorLen = separator.Length;
                    if (sbParams.Length >= separatorLen)
                    {
                        sbParams.Remove(sbParams.Length - separatorLen, separatorLen);
                    }

                    Logger.DebugFormat("sql: {0}; params: {1};", commandText, sbParams.ToString());
                }
                catch (Exception ex)
                {
                    Logger.Error("params SqlParameter", ex);
                }
            }
        }

        #endregion

        #region 輸出資料集

        public virtual DataSet ExecuteDataset()
        {
            DataSet ds = null;
            bool hasParameters = false;
            bool hasOutputParameters = false;

            //動態取得衍生類別的成員變數
            FieldInfo[] fields = this.GetType().GetFields();

            if (fields.Length > 0)
                hasParameters = true;

            foreach (FieldInfo field in fields)
            {
                object[] results = field.GetCustomAttributes(typeof(OutputParaAttribute), false);

                //todo by lozen
                object result = null;
                result = field.GetValue(this);
                //field.FieldType

                if (results.Length > 0)
                    hasOutputParameters = true;
            }

            if (hasParameters)
            {
                if (hasOutputParameters)
                {
                    //輸出資料集,有參數,輸出參數內容
                    int RowCount = 0, BeginNum = 0, EndNum = 0, ListMode = 0;   //lozentest
                    string pa1 = "", pa2 = "", SortField = "";  //lozentest
                    bool IsSortDesc = false;    //lozentest
                    try
                    {
                        //預設總列數為錯誤
                        RowCount = -1;

                        //建立回傳參數
                        SqlParameter paRowCount = new SqlParameter("@RowCount", SqlDbType.Int);
                        paRowCount.Direction = ParameterDirection.Output;

                        //建立連線資訊並開啟連線
                        conn = db.CreateConnectionInstanceWithOpen();

                        LogSql("spDemo_GetList",
                            new SqlParameter("@pa1", pa1),
                            new SqlParameter("@pa2", pa2),
                            new SqlParameter("@ListMode", ListMode),
                            new SqlParameter("@BeginNum", BeginNum),
                            new SqlParameter("@EndNum", EndNum),
                            new SqlParameter("@SortField", SortField),
                            new SqlParameter("@IsSortDesc", IsSortDesc),
                            paRowCount);

                        SqlHelper.FillDataset(conn, CommandType.StoredProcedure, "dbo.spDemo_GetList", ds,
                            new string[] { "spDemo_GetList" },
                            new SqlParameter("@pa1", pa1),
                            new SqlParameter("@pa2", pa2),
                            new SqlParameter("@ListMode", ListMode),
                            new SqlParameter("@BeginNum", BeginNum),
                            new SqlParameter("@EndNum", EndNum),
                            new SqlParameter("@SortField", SortField),
                            new SqlParameter("@IsSortDesc", IsSortDesc),
                            paRowCount);

                        //取得總列數
                        RowCount = Convert.ToInt32(paRowCount.Value);
                    }
                    catch (Exception ex)
                    {
                        Logger.Error("", ex);

                        //回傳錯誤訊息
                        ErrMsg = ex.Message;
                        return null;
                    }
                    finally
                    {
                        //關閉連線資訊
                        db.CloseConnection(conn);
                    }
                }
                else
                {
                    //輸出資料集,有參數
                    string pa1 = "", pa2 = "";  //lozentest
                    try
                    {
                        //建立連線資訊並開啟連線
                        conn = db.CreateConnectionInstanceWithOpen();

                        LogSql("spDemo_GetList", pa1, pa2);

                        SqlHelper.FillDataset(conn, "dbo.spDemo_GetList", ds,
                            new string[] { "spDemo_GetList" }, pa1, pa2);
                    }
                    catch (Exception ex)
                    {
                        Logger.Error("", ex);

                        //回傳錯誤訊息
                        ErrMsg = ex.Message;
                        return null;
                    }
                    finally
                    {
                        //關閉連線資訊
                        db.CloseConnection(conn);
                    }
                }
            }
            else
            {
                //輸出資料集,無參數
                try
                {
                    //建立連線資訊並開啟連線
                    conn = db.CreateConnectionInstanceWithOpen();

                    LogSql("spDemo_GetList", "");

                    SqlHelper.FillDataset(conn, CommandType.StoredProcedure, "dbo.spDemo_GetList", ds,
                        new string[] { "spDemo_GetList" });
                }
                catch (Exception ex)
                {
                    Logger.Error("", ex);

                    //回傳錯誤訊息
                    ErrMsg = ex.Message;
                    return null;
                }
                finally
                {
                    //關閉連線資訊
                    db.CloseConnection(conn);
                }
            }
            

            return ds;
        }

        #endregion
    }

    /// <summary>
    /// 宣告成員變數為輸出參數
    /// </summary>
    public class OutputParaAttribute : Attribute
    {
        public OutputParaAttribute()
        {
        }
    }

    /// <summary>
    /// 取得後端使用者登入用資料
    /// </summary>
    public class spEmployee_GetDataToLogin : DataAccessCommandBase
    {
        public string EmpAccount;
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
