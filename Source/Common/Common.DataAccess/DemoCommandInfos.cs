using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Common.DataAccess;
using System.Data.SqlClient;
using log4net;
using Microsoft.ApplicationBlocks.Data;

namespace Common.DataAccess.DemoCommandInfos
{
    //範例用; Demo

    #region 讓 DataAccessCommand 使用欄位變數; DataAccessCommand generates SqlParameter information from fields automatically.

    /// <summary>
    /// 無參數; without parameter.
    /// </summary>
    public class spDemo_WoPara : IDataAccessCommandInfo
    {
        public CommandType GetCommandType()
        {
            return CommandType.StoredProcedure;
        }

        public string GetCommandText()
        {
            return "dbo.spDemo_WoPara";
        }
    }

    /// <summary>
    /// 有參數; with parameters.
    /// </summary>
    public class spDemo_WithPara : IDataAccessCommandInfo
    {
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value) from these fields automatically. Property is not included.
        // Output parameter needs attribute [OutputPara]
        public int pa1;
        public string pa2;

        public CommandType GetCommandType()
        {
            return CommandType.StoredProcedure;
        }

        public string GetCommandText()
        {
            return "dbo.spDemo_WithPara";
        }
    }

    /// <summary>
    /// 有參數,輸出參數內容; with parameters, include output parameter.
    /// </summary>
    public class spDemo_WithOutputPara : IDataAccessCommandInfo
    {
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value) from these fields automatically. Property is not included.
        // Output parameter needs attribute [OutputPara]
        public int pa1;
        public string pa2;
        public int ListMode;
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
            return "dbo.spDemo_WithOutputPara";
        }
    }

    /// <summary>
    /// 有參數(加權限參數),輸出參數內容; with parameters, include output parameter and authentication parameter.
    /// </summary>
    public class spDemo_WithOutputParaAndAuthPara : IDataAccessCommandInfo
    {
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value) from these fields automatically. Property is not included.
        // Output parameter needs attribute [OutputPara]
        public int pa1;
        public string pa2;
        public int ListMode;
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
            return "dbo.spDemo_WithOutputParaAndAuthPara";
        }
    }

    #endregion

    #region 在執行sql指令前異動參數內容; custom modify CommandParameters before execute.

    /// <summary>
    /// 在執行sql指令前異動參數內容; custom modify CommandParameters before execute.
    /// </summary>
    public class spDemo_ModifyCommandParametersBeforeExecute : IDataAccessCommandInfo, IModifyCommandParametersBeforeExecute
    {
        // DataAccessCommand 會使用欄位變數當做 SqlParameter 的產生來源(使用名稱、值)；屬性不包含在其中。
        // 輸出參數請加上屬性 [OutputPara]
        // DataAccessCommand generates SqlParameter information(name, value) from these fields automatically. Property is not included.
        // Output parameter needs attribute [OutputPara]
        public int pa1;
        public string pa2;
        public int? paNullable;

        public CommandType GetCommandType()
        {
            return CommandType.StoredProcedure;
        }

        public string GetCommandText()
        {
            return "dbo.spDemo_ModifyCommandParametersBeforeExecute";
        }

        public void ModifyCommandParametersBeforeExecute(SqlParameter[] commandParameters)
        {
            foreach (SqlParameter pa in commandParameters)
            {
                switch (pa.ParameterName)
                {
                    case "@paNullable":
                        if (!paNullable.HasValue)
                        {
                            pa.Value = -1;
                        }
                        break;
                }
            }
        }
    }

    #endregion

    #region 自訂執行功能; custom execute function.

    /// <summary>
    /// 自訂取回 DataSet 的執行功能; custom execute function that returns DataSet.
    /// </summary>
    public class spDemo_CustomExecuteDataset : IDataAccessCommandInfo, ICustomExecuteDataset
    {
        public int pa1;
        public string pa2;

        public CommandType GetCommandType()
        {
            return CommandType.StoredProcedure;
        }

        public string GetCommandText()
        {
            return "dbo.spDemo_CustomExecuteDataset";
        }

        public DataSet ExecuteDataset(IDataAccessCommandInnerTools innerTools)
        {
            ILog Logger = innerTools.GetLogger();
            IDataAccessSource db = innerTools.GetDataAccessSource();
            SqlConnection conn = null;
            DataSet ds = null;

            try
            {
                //建立連線資訊並開啟連線
                conn = db.CreateConnectionInstanceWithOpen();

                innerTools.SetLogSql("spDemo_GetList", pa1, pa2);

                ds = SqlHelper.ExecuteDataset(conn, "dbo.spDemo_GetList", pa1, pa2);
            }
            catch (Exception ex)
            {
                Logger.Error("", ex);

                //回傳錯誤訊息
                innerTools.SetErrMsg(ex.Message);
                return null;
            }
            finally
            {
                //關閉連線資訊
                db.CloseConnection(conn);
            }

            return ds;
        }
    }

    /// <summary>
    /// 自訂取回 DataSet 的執行功能,輸出參數內容; custom execute function that returns DataSet and output parameter.
    /// </summary>
    public class spDemo_CustomExecuteDatasetWithOutputPara : IDataAccessCommandInfo, ICustomExecuteDataset
    {
        public int pa1;
        public string pa2;
        public int ListMode;
        public int BeginNum;
        public int EndNum;
        public string SortField;
        public bool IsSortDesc;
        public int RowCount;

        public CommandType GetCommandType()
        {
            return CommandType.StoredProcedure;
        }

        public string GetCommandText()
        {
            return "dbo.spDemo_CustomExecuteDatasetWithOutputPara";
        }

        public DataSet ExecuteDataset(IDataAccessCommandInnerTools innerTools)
        {
            ILog Logger = innerTools.GetLogger();
            IDataAccessSource db = innerTools.GetDataAccessSource();
            SqlConnection conn = null;
            DataSet ds = null;

            try
            {
                //建立連線資訊並開啟連線
                conn = db.CreateConnectionInstanceWithOpen();

                //建立回傳參數
                SqlParameter paRowCount = new SqlParameter("@RowCount", SqlDbType.Int);
                paRowCount.Direction = ParameterDirection.Output;

                innerTools.SetLogSql("spDemo_CustomExecuteDatasetWithOutputPara",
                    new SqlParameter("@pa1", pa1),
                    new SqlParameter("@pa2", pa2),
                    new SqlParameter("@ListMode", ListMode),
                    new SqlParameter("@BeginNum", BeginNum),
                    new SqlParameter("@EndNum", EndNum),
                    new SqlParameter("@SortField", SortField),
                    new SqlParameter("@IsSortDesc", IsSortDesc),
                    paRowCount);

                ds = SqlHelper.ExecuteDataset(conn, CommandType.StoredProcedure, "dbo.spDemo_CustomExecuteDatasetWithOutputPara",
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
                innerTools.SetErrMsg(ex.Message);
                return null;
            }
            finally
            {
                //關閉連線資訊
                db.CloseConnection(conn);
            }

            return ds;
        }
    }

    /// <summary>
    /// 自訂執行功能; custom execute function.
    /// </summary>
    public class spDemo_CustomExecuteNonQuery : IDataAccessCommandInfo, ICustomExecuteNonQuery
    {
        public int pa1;
        public string pa2;

        public CommandType GetCommandType()
        {
            return CommandType.StoredProcedure;
        }

        public string GetCommandText()
        {
            return "dbo.spDemo_CustomExecuteNonQuery";
        }

        public bool ExecuteNonQuery(IDataAccessCommandInnerTools innerTools)
        {
            ILog Logger = innerTools.GetLogger();
            IDataAccessSource db = innerTools.GetDataAccessSource();
            SqlConnection conn = null;

            try
            {
                //建立連線資訊並開啟連線
                conn = db.CreateConnectionInstanceWithOpen();

                innerTools.SetLogSql("spDemo_CustomExecuteNonQuery", pa1, pa2);

                SqlHelper.ExecuteNonQuery(conn, "dbo.spDemo_CustomExecuteNonQuery", pa1, pa2);
            }
            catch (Exception ex)
            {
                Logger.Error("", ex);

                //回傳錯誤訊息
                innerTools.SetErrMsg(ex.Message);
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

    /// <summary>
    /// 自訂執行功能,輸出參數內容; custom execute function that returns output parameter.
    /// </summary>
    public class spDemo_CustomExecuteNonQueryWithOutputPara : IDataAccessCommandInfo, ICustomExecuteNonQuery
    {
        public int pa1;
        public string pa2;
        public int OutId;

        public CommandType GetCommandType()
        {
            return CommandType.StoredProcedure;
        }

        public string GetCommandText()
        {
            return "dbo.spDemo_CustomExecuteNonQueryWithOutputPara";
        }

        public bool ExecuteNonQuery(IDataAccessCommandInnerTools innerTools)
        {
            ILog Logger = innerTools.GetLogger();
            IDataAccessSource db = innerTools.GetDataAccessSource();
            SqlConnection conn = null;

            try
            {
                //建立連線資訊並開啟連線
                conn = db.CreateConnectionInstanceWithOpen();

                //預設回傳結果
                OutId = -1;

                //建立回傳參數
                SqlParameter paOutId = new SqlParameter("@OutId", SqlDbType.Int);
                paOutId.Direction = ParameterDirection.Output;

                innerTools.SetLogSql("spDemo_CustomExecuteNonQueryWithOutputPara",
                    new SqlParameter("@pa1", pa1),
                    new SqlParameter("@pa2", pa2),
                    paOutId);

                SqlHelper.ExecuteNonQuery(conn, CommandType.StoredProcedure, "dbo.spDemo_CustomExecuteNonQueryWithOutputPara",
                    new SqlParameter("@pa1", pa1),
                    new SqlParameter("@pa2", pa2),
                    paOutId);

                //回傳結果
                OutId = Convert.ToInt32(paOutId.Value);
            }
            catch (Exception ex)
            {
                Logger.Error("", ex);

                //回傳錯誤訊息
                innerTools.SetErrMsg(ex.Message);
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

    /// <summary>
    /// 自訂取回第一個欄位值的執行功能; custom execute function that returns first field.
    /// </summary>
    public class spDemo_CustomExecuteScalar : IDataAccessCommandInfo, ICustomExecuteScalar
    {
        public int pa1;
        public string pa2;

        public CommandType GetCommandType()
        {
            return CommandType.StoredProcedure;
        }

        public string GetCommandText()
        {
            return "dbo.spDemo_CustomExecuteScalar";
        }

        public T ExecuteScalar<T>(IDataAccessCommandInnerTools innerTools, T errCode)
        {
            ILog Logger = innerTools.GetLogger();
            IDataAccessSource db = innerTools.GetDataAccessSource();
            SqlConnection conn = null;
            T result = default(T);

            try
            {
                //建立連線資訊並開啟連線
                conn = db.CreateConnectionInstanceWithOpen();

                innerTools.SetLogSql("spDemo_CustomExecuteScalar", pa1, pa2);

                result = (T)SqlHelper.ExecuteScalar(conn, "dbo.spDemo_CustomExecuteScalar", pa1, pa2);
            }
            catch (Exception ex)
            {
                Logger.Error("", ex);

                //回傳錯誤訊息
                innerTools.SetErrMsg(ex.Message);
                return errCode;
            }
            finally
            {
                //關閉連線資訊
                db.CloseConnection(conn);
            }

            return result;
        }
    }

    /// <summary>
    /// 自訂取回 DataReader 的執行功能; custom execute function that returns DataReader.
    /// </summary>
    public class spDemo_CustomExecuteReader : IDataAccessCommandInfo, ICustomExecuteReader
    {
        public int pa1;
        public string pa2;

        public CommandType GetCommandType()
        {
            return CommandType.StoredProcedure;
        }

        public string GetCommandText()
        {
            return "dbo.spDemo_CustomExecuteReader";
        }

        public IDataReader ExecuteReader(IDataAccessCommandInnerTools innerTools, out SqlConnection connOut)
        {
            ILog Logger = innerTools.GetLogger();
            IDataAccessSource db = innerTools.GetDataAccessSource();
            SqlConnection conn = null;
            IDataReader rdr = null;

            try
            {
                //建立連線資訊並開啟連線
                conn = db.CreateConnectionInstanceWithOpen();

                innerTools.SetLogSql("spDemo_GetList", pa1, pa2);

                rdr = SqlHelper.ExecuteReader(conn, "dbo.spDemo_GetList", pa1, pa2);

                //輸出連線資訊
                connOut = conn;
            }
            catch (Exception ex)
            {
                Logger.Error("", ex);

                //回傳錯誤訊息
                innerTools.SetErrMsg(ex.Message);
                //失敗時關閉連線
                db.CloseConnection(conn);
                connOut = null;

                return null;
            }

            return rdr;
        }
    }

    #endregion
}
