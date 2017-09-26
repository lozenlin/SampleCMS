using System.Data;
using System.Data.SqlClient;

namespace Common.DataAccess
{
    /// <summary>
    /// 資料存取來源
    /// </summary>
    public interface IDataAccessSource
    {
        /// <summary>
        /// 取得連線字串提取名稱
        /// </summary>
        string GetConnectionStringName();
        /// <summary>
        /// 設定連線字串提取名稱
        /// </summary>
        void SetConnectionStringName(string connectionStringName);
        /// <summary>
        /// 建立連線資訊
        /// </summary>
        SqlConnection CreateConnectionInstance();
        /// <summary>
        /// 建立連線資訊並開啟連線
        /// </summary>
        SqlConnection CreateConnectionInstanceWithOpen();
        /// <summary>
        /// 關閉連線資訊
        /// </summary>
        void CloseConnection(IDbConnection conn);
        /// <summary>
        /// 關閉DataReader
        /// </summary>
        void CloseDataReader(IDataReader rdr, IDbConnection conn);
    }

    /// <summary>
    /// 資料庫存取指令執行者
    /// </summary>
    public interface IDataAccessCommand
    {
        /// <summary>
        /// 執行指令並取回 DataSet
        /// </summary>
        DataSet ExecuteDataset(IDataAccessCommandInfo cmdInfo);
        /// <summary>
        /// 執行後的錯誤訊息
        /// </summary>
        string GetErrMsg();
    }

    /// <summary>
    /// 資料庫存取指令資訊
    /// </summary>
    public interface IDataAccessCommandInfo
    {
        /// <summary>
        /// 指令類型
        /// </summary>
        CommandType GetCommandType();
        /// <summary>
        /// 指令內容
        /// </summary>
        string GetCommandText();
    }

    /// <summary>
    /// 在執行sql指令前異動參數內容
    /// </summary>
    public interface IModifyCommandParametersBeforeExecute
    {
        /// <summary>
        /// 在執行sql指令前異動參數內容
        /// </summary>
        void ModifyCommandParametersBeforeExecute(SqlParameter[] commandParameters);
    }

    /// <summary>
    /// 自訂取回 DataSet 的執行功能
    /// </summary>
    public interface ICustomExecuteDataset
    {
        /// <summary>
        /// 執行指令並取回 DataSet
        /// </summary>
        DataSet ExecuteDataset();
    }
}
