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
    /// 基礎資料庫存取指令功能
    /// </summary>
    public abstract class DataAccessCommandBase
    {
        /// <summary>
        /// 記錄工具
        /// </summary>
        protected ILog Logger
        {
            get;
            set;
        }

        protected DataAccessSource db = null;
        protected CommandType cmdType = CommandType.Text;
        protected string cmdText = "";
        protected string errMsg = "";
        protected string typeName = "";

        private SqlConnection conn = null;

        public DataAccessCommandBase(DataAccessSource dataAccessSource)
        {
            db = dataAccessSource;
            Logger = LogManager.GetLogger(this.GetType());
            typeName = this.GetType().Name;
        }

        /// <summary>
        /// 執行後的錯誤訊息
        /// </summary>
        public string GetErrMsg()
        {
            return errMsg;
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

            //動態取得物件的公用欄位
            FieldInfo[] fields = this.GetType().GetFields();
            List<SqlParaInfo> paraInfos = new List<SqlParaInfo>();

            if (fields.Length > 0)
                hasParameters = true;

            foreach (FieldInfo field in fields)
            {
                object[] outputAttrs = field.GetCustomAttributes(typeof(OutputParaAttribute), false);

                object fieldValue = null;
                fieldValue = field.GetValue(this);
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

            if (hasParameters)
            {
                //輸出資料集,有參數
                try
                {
                    //建立連線資訊並開啟連線
                    conn = db.CreateConnectionInstanceWithOpen();

                    SqlParameter[] commandParameters = GenerateCommandParameters(paraInfos);

                    ModifyParaInfosBeforeExecute(paraInfos);

                    LogSql(cmdText, commandParameters);

                    ds = SqlHelper.ExecuteDataset(conn, cmdType, cmdText,
                        commandParameters);

                    //取得輸出參數值
                    if (hasOutputParameters)
                    {
                        foreach (SqlParaInfo paraInfo in paraInfos)
                        {
                            if (paraInfo.IsOutput)
                            {
                                FieldInfo outputParaFieldInfo = paraInfo.ParaFieldInfo;
                                outputParaFieldInfo.SetValue(this, paraInfo.SqlPara.Value);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error("", ex);

                    //回傳錯誤訊息
                    errMsg = ex.Message;
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
                //輸出資料集,無參數
                try
                {
                    //建立連線資訊並開啟連線
                    conn = db.CreateConnectionInstanceWithOpen();

                    LogSql(cmdText, "");

                    ds = SqlHelper.ExecuteDataset(conn, cmdType, cmdText);
                }
                catch (Exception ex)
                {
                    Logger.Error("", ex);

                    //回傳錯誤訊息
                    errMsg = ex.Message;
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

        /// <summary>
        /// 在執行sql指令前異動參數內容
        /// </summary>
        protected virtual void ModifyParaInfosBeforeExecute(List<SqlParaInfo> paraInfos)
        {
        }

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
    }
}
