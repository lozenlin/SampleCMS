// ===============================================================================
// DataAccessCommand of SampleCMS
// https://github.com/lozenlin/SampleCMS
//
// DataAccessCommand.cs
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
using System.Reflection;
using System.Text;

namespace Common.DataAccess
{
    /// <summary>
    /// 資料庫存取指令執行者工廠
    /// </summary>
    public class DataAccessCommandFactory
    {
        public DataAccessCommandFactory()
        {
        }

        /// <summary>
        /// 取得資料庫存取指令執行者
        /// </summary>
        public static IDataAccessCommand GetDataAccessCommand(IDataAccessSource dataAccessSource)
        {
            return new DataAccessCommand(dataAccessSource);
        }
    }

    /// <summary>
    /// 資料庫存取指令執行者
    /// </summary>
    public class DataAccessCommand : IDataAccessCommand, IDataAccessCommandInnerTools
    {
        /// <summary>
        /// 記錄工具
        /// </summary>
        protected ILog Logger
        {
            get;
            set;
        }
        
        protected IDataAccessSource db = null;
        protected bool hasParameters = false;
        protected bool hasOutputParameters = false;
        protected string errMsg = "";
        protected int sqlErrNumber = 0;
        protected int sqlErrState = 0;

        private SqlConnection conn = null;

        public DataAccessCommand(IDataAccessSource dataAccessSource)
        {
            db = dataAccessSource;
            Logger = LogManager.GetLogger(this.GetType());
        }

        /// <summary>
        /// 執行後的錯誤訊息
        /// </summary>
        public string GetErrMsg()
        {
            return errMsg;
        }

        public int GetSqlErrNumber()
        {
            return sqlErrNumber;
        }

        public int GetSqlErrState()
        {
            return sqlErrState;
        }

        #region LogSql

        /// <summary>
        /// 記錄送去資料庫的指令和參數值
        /// </summary>
        protected void LogSql(string commandText, params object[] parameterValues)
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
        protected void LogSql(string commandText, params SqlParameter[] commandParameters)
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

        /// <summary>
        /// 執行指令並取回 DataSet
        /// </summary>
        public DataSet ExecuteDataset(IDataAccessCommandInfo cmdInfo)
        {
            //若有自訂取回 DataSet 的執行功能就轉用自訂的
            if (cmdInfo is ICustomExecuteDataset)
            {
                return ((ICustomExecuteDataset)cmdInfo).ExecuteDataset(this);
            }

            CommandType cmdType = cmdInfo.GetCommandType();
            string cmdText = cmdInfo.GetCommandText();
            DataSet ds = null;

            //動態取得物件的公用欄位
            List<SqlParaInfo> paraInfos = GenerateSqlParaInfos(cmdInfo);

            try
            {
                //建立連線資訊並開啟連線
                conn = db.CreateConnectionInstanceWithOpen();

                SqlParameter[] commandParameters = GenerateCommandParameters(paraInfos);

                //在執行sql指令前異動參數內容
                if (cmdInfo is IModifyCommandParametersBeforeExecute)
                {
                    ((IModifyCommandParametersBeforeExecute)cmdInfo).ModifyCommandParametersBeforeExecute(commandParameters);
                }

                LogSql(cmdText, commandParameters);

                if (hasParameters)
                {
                    ds = SqlHelper.ExecuteDataset(conn, cmdType, cmdText,
                        commandParameters);
                }
                else
                {
                    ds = SqlHelper.ExecuteDataset(conn, cmdType, cmdText);
                }

                //取得輸出參數值
                if (hasOutputParameters)
                {
                    UpdateOutputParameterValuesOfSqlParaInfosFromSqlParameter(paraInfos, cmdInfo);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("", ex);

                //回傳錯誤訊息
                errMsg = ex.Message;

                if (ex is SqlException)
                {
                    SqlException sqlex = (SqlException)ex;
                    sqlErrNumber = sqlex.Number;
                    sqlErrState = sqlex.State;
                }

                return null;
            }
            finally
            {
                //關閉連線資訊
                db.CloseConnection(conn);
            }

            return ds;
        }

        #endregion

        #region 無輸出

        /// <summary>
        /// 執行指令
        /// </summary>
        public bool ExecuteNonQuery(IDataAccessCommandInfo cmdInfo)
        {
            //若有自訂執行功能就轉用自訂的
            if (cmdInfo is ICustomExecuteNonQuery)
            {
                return ((ICustomExecuteNonQuery)cmdInfo).ExecuteNonQuery(this);
            }

            CommandType cmdType = cmdInfo.GetCommandType();
            string cmdText = cmdInfo.GetCommandText();

            //動態取得物件的公用欄位
            List<SqlParaInfo> paraInfos = GenerateSqlParaInfos(cmdInfo);

            try
            {
                //建立連線資訊並開啟連線
                conn = db.CreateConnectionInstanceWithOpen();

                SqlParameter[] commandParameters = GenerateCommandParameters(paraInfos);

                //在執行sql指令前異動參數內容
                if (cmdInfo is IModifyCommandParametersBeforeExecute)
                {
                    ((IModifyCommandParametersBeforeExecute)cmdInfo).ModifyCommandParametersBeforeExecute(commandParameters);
                }

                LogSql(cmdText, commandParameters);

                if (hasParameters)
                {
                    SqlHelper.ExecuteNonQuery(conn, cmdType, cmdText,
                        commandParameters);
                }
                else
                {
                    SqlHelper.ExecuteNonQuery(conn, cmdType, cmdText);
                }

                //取得輸出參數值
                if (hasOutputParameters)
                {
                    UpdateOutputParameterValuesOfSqlParaInfosFromSqlParameter(paraInfos, cmdInfo);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("", ex);

                //回傳錯誤訊息
                errMsg = ex.Message;

                if (ex is SqlException)
                {
                    SqlException sqlex = (SqlException)ex;
                    sqlErrNumber = sqlex.Number;
                    sqlErrState = sqlex.State;
                }

                return false;
            }
            finally
            {
                //關閉連線資訊
                db.CloseConnection(conn);
            }

            return true;
        }

        #endregion

        #region 輸出有型別內容(int,string,...)

        /// <summary>
        /// 執行指令並取回第一個欄位值
        /// </summary>
        /// <param name="errCode">做為錯誤碼的值</param>
        public T ExecuteScalar<T>(IDataAccessCommandInfo cmdInfo, T errCode)
        {
            //若有自訂執行功能就轉用自訂的
            if (cmdInfo is ICustomExecuteScalar)
            {
                return ((ICustomExecuteScalar)cmdInfo).ExecuteScalar<T>(this, errCode);
            }

            T result = default(T);
            CommandType cmdType = cmdInfo.GetCommandType();
            string cmdText = cmdInfo.GetCommandText();

            //動態取得物件的公用欄位
            List<SqlParaInfo> paraInfos = GenerateSqlParaInfos(cmdInfo);

            try
            {
                //建立連線資訊並開啟連線
                conn = db.CreateConnectionInstanceWithOpen();

                SqlParameter[] commandParameters = GenerateCommandParameters(paraInfos);

                //在執行sql指令前異動參數內容
                if (cmdInfo is IModifyCommandParametersBeforeExecute)
                {
                    ((IModifyCommandParametersBeforeExecute)cmdInfo).ModifyCommandParametersBeforeExecute(commandParameters);
                }

                LogSql(cmdText, commandParameters);

                if (hasParameters)
                {
                    result = (T)SqlHelper.ExecuteScalar(conn, cmdType, cmdText,
                        commandParameters);
                }
                else
                {
                    result = (T)SqlHelper.ExecuteScalar(conn, cmdType, cmdText);
                }

                //取得輸出參數值
                if (hasOutputParameters)
                {
                    UpdateOutputParameterValuesOfSqlParaInfosFromSqlParameter(paraInfos, cmdInfo);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("", ex);

                //回傳錯誤訊息
                errMsg = ex.Message;

                if (ex is SqlException)
                {
                    SqlException sqlex = (SqlException)ex;
                    sqlErrNumber = sqlex.Number;
                    sqlErrState = sqlex.State;
                }

                return errCode;
            }
            finally
            {
                //關閉連線資訊
                db.CloseConnection(conn);
            }

            return result;
        }

        #endregion

        #region 輸出 DataReader

        /// <summary>
        /// 執行指令並取回 DataReader
        /// </summary>
        public IDataReader ExecuteReader(IDataAccessCommandInfo cmdInfo, out SqlConnection connOut)
        {
            //若有自訂執行功能就轉用自訂的
            if (cmdInfo is ICustomExecuteReader)
            {
                return ((ICustomExecuteReader)cmdInfo).ExecuteReader(this, out connOut);
            }

            CommandType cmdType = cmdInfo.GetCommandType();
            string cmdText = cmdInfo.GetCommandText();
            IDataReader rdr = null;
            
            //動態取得物件的公用欄位
            List<SqlParaInfo> paraInfos = GenerateSqlParaInfos(cmdInfo);

            try
            {
                //建立連線資訊並開啟連線
                conn = db.CreateConnectionInstanceWithOpen();

                SqlParameter[] commandParameters = GenerateCommandParameters(paraInfos);

                //在執行sql指令前異動參數內容
                if (cmdInfo is IModifyCommandParametersBeforeExecute)
                {
                    ((IModifyCommandParametersBeforeExecute)cmdInfo).ModifyCommandParametersBeforeExecute(commandParameters);
                }

                LogSql(cmdText, commandParameters);

                if (hasParameters)
                {
                    rdr = SqlHelper.ExecuteReader(conn, cmdType, cmdText,
                        commandParameters);
                }
                else
                {
                    rdr = SqlHelper.ExecuteReader(conn, cmdType, cmdText);
                }

                //輸出連線資訊
                connOut = conn;

                //取得輸出參數值
                if (hasOutputParameters)
                {
                    UpdateOutputParameterValuesOfSqlParaInfosFromSqlParameter(paraInfos, cmdInfo);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("", ex);

                //回傳錯誤訊息
                errMsg = ex.Message;

                if (ex is SqlException)
                {
                    SqlException sqlex = (SqlException)ex;
                    sqlErrNumber = sqlex.Number;
                    sqlErrState = sqlex.State;
                }

                //失敗時關閉連線
                db.CloseConnection(conn);
                connOut = null;

                return null;
            }

            return rdr;
        }

        #endregion

        /// <summary>
        /// 動態取得物件的公用欄位當做指令參數
        /// </summary>
        protected List<SqlParaInfo> GenerateSqlParaInfos(IDataAccessCommandInfo cmdInfo)
        {
            //動態取得物件的公用欄位
            FieldInfo[] fields = cmdInfo.GetType().GetFields();
            List<SqlParaInfo> paraInfos = new List<SqlParaInfo>();

            if (fields.Length > 0)
                hasParameters = true;

            foreach (FieldInfo field in fields)
            {
                object[] outputAttrs = field.GetCustomAttributes(typeof(OutputParaAttribute), false);

                object fieldValue = null;
                fieldValue = field.GetValue(cmdInfo);
                bool isOutputPara = false;

                if (outputAttrs.Length > 0)
                {
                    isOutputPara = true;
                    hasOutputParameters = true;
                }

                paraInfos.Add(new SqlParaInfo()
                {
                    Name = field.Name,
                    Value = fieldValue,
                    IsOutput = isOutputPara,
                    ParaType = field.FieldType,
                    ParaFieldInfo = field
                });
            }

            return paraInfos;
        }

        /// <summary>
        /// 從 SqlParameter 更新參數資訊清單中的所有輸出參數值
        /// </summary>
        protected void UpdateOutputParameterValuesOfSqlParaInfosFromSqlParameter(List<SqlParaInfo> paraInfos, IDataAccessCommandInfo cmdInfo)
        {
            foreach (SqlParaInfo paraInfo in paraInfos)
            {
                if (paraInfo.IsOutput)
                {
                    FieldInfo outputParaFieldInfo = paraInfo.ParaFieldInfo;
                    outputParaFieldInfo.SetValue(cmdInfo, paraInfo.SqlPara.Value);
                }
            }
        }

        /// <summary>
        /// 用 SqlParaInfo 產生 SqlParameter
        /// </summary>
        protected SqlParameter[] GenerateCommandParameters(List<SqlParaInfo> paraInfos)
        {
            List<SqlParameter> sqlParas = new List<SqlParameter>();

            foreach (SqlParaInfo paraInfo in paraInfos)
            {
                SqlParameter sqlPara = new SqlParameter("@" + paraInfo.Name, paraInfo.Value);

                if (paraInfo.IsOutput)
                    sqlPara.Direction = ParameterDirection.Output;

                paraInfo.SqlPara = sqlPara;

                sqlParas.Add(sqlPara);
            }

            return sqlParas.ToArray();
        }

        #region IDataAccessCommandInnerTools

        public IDataAccessSource GetDataAccessSource()
        {
            return db;
        }

        public ILog GetLogger()
        {
            return Logger;
        }

        public void SetErrMsg(string errMsg)
        {
            this.errMsg = errMsg;
        }

        public void SetLogSql(string commandText, params object[] parameterValues)
        {
            LogSql(commandText, parameterValues);
        }

        public void SetLogSql(string commandText, params SqlParameter[] commandParameters)
        {
            LogSql(commandText, commandParameters);
        }

        #endregion

    }
}
