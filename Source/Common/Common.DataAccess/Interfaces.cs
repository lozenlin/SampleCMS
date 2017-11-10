// ===============================================================================
// Interfaces of DataAccessCommand of SampleCMS
// https://github.com/lozenlin/SampleCMS
//
// Interfaces.cs
//
// ===============================================================================
// Copyright (c) 2017 lozenlin
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// ===============================================================================

using log4net;
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
        /// 執行指令
        /// </summary>
        bool ExecuteNonQuery(IDataAccessCommandInfo cmdInfo);
        /// <summary>
        /// 執行指令並取回第一個欄位值
        /// </summary>
        /// <param name="errCode">做為錯誤碼的值</param>
        T ExecuteScalar<T>(IDataAccessCommandInfo cmdInfo, T errCode);
        /// <summary>
        /// 執行指令並取回 DataReader
        /// </summary>
        IDataReader ExecuteReader(IDataAccessCommandInfo cmdInfo, out SqlConnection connOut);
        /// <summary>
        /// 執行後的錯誤訊息
        /// </summary>
        string GetErrMsg();
        /// <summary>
        /// 執行後 SqlServer 回傳的 error number
        /// </summary>
        int GetSqlErrNumber();
        /// <summary>
        /// 執行後 SqlServer 回傳的 error state
        /// </summary>
        int GetSqlErrState();
    }

    /// <summary>
    /// 資料庫存取指令執行者內部工具
    /// </summary>
    public interface IDataAccessCommandInnerTools
    {
        /// <summary>
        /// 取得資料存取來源
        /// </summary>
        IDataAccessSource GetDataAccessSource();
        /// <summary>
        /// 取得記錄器
        /// </summary>
        ILog GetLogger();
        /// <summary>
        /// 設定錯誤訊息
        /// </summary>
        void SetErrMsg(string errMsg);
        /// <summary>
        /// 記錄送去資料庫的指令和參數值
        /// </summary>
        void SetLogSql(string commandText, params object[] parameterValues);
        /// <summary>
        /// 記錄送去資料庫的指令和參數值
        /// </summary>
        void SetLogSql(string commandText, params SqlParameter[] commandParameters);
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
        DataSet ExecuteDataset(IDataAccessCommandInnerTools innerTools);
    }

    /// <summary>
    /// 自訂執行功能
    /// </summary>
    public interface ICustomExecuteNonQuery
    {
        /// <summary>
        /// 執行指令
        /// </summary>
        bool ExecuteNonQuery(IDataAccessCommandInnerTools innerTools);
    }

    /// <summary>
    /// 自訂取回第一個欄位值的執行功能
    /// </summary>
    public interface ICustomExecuteScalar
    {
        /// <summary>
        /// 執行指令並取回第一個欄位值
        /// </summary>
        /// <param name="errCode">做為錯誤碼的值</param>
        T ExecuteScalar<T>(IDataAccessCommandInnerTools innerTools, T errCode);
    }

    /// <summary>
    /// 自訂取回 DataReader 的執行功能
    /// </summary>
    public interface ICustomExecuteReader
    {
        /// <summary>
        /// 執行指令並取回 DataReader
        /// </summary>
        IDataReader ExecuteReader(IDataAccessCommandInnerTools innerTools, out SqlConnection connOut);
    }
}
