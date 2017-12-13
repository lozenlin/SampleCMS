// ===============================================================================
// DataAccessSource of SampleCMS
// https://github.com/lozenlin/SampleCMS
//
// DataAccessSource.cs
//
// ===============================================================================
// Copyright (c) 2017 lozenlin
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// ===============================================================================

using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Common.DataAccess
{
    /// <summary>
    /// 資料存取來源
    /// </summary>
    public class DataAccessSource : IDataAccessSource
    {
        /// <summary>
        /// 連線字串提取名稱
        /// </summary>
        private string connectionStringName = "DBConnString";

        /// <summary>
        /// connectionString is default value "DBConnString"
        /// </summary>
        public DataAccessSource()
        {
        }

        public DataAccessSource(string connectionStringName)
        {
            this.connectionStringName = connectionStringName;
        }

        #region 連線字串相關 ConnectionStringName

        /// <summary>
        /// 取得連線字串提取名稱
        /// </summary>
        public virtual string GetConnectionStringName()
        {
            return connectionStringName;
        }

        /// <summary>
        /// 設定連線字串提取名稱
        /// </summary>
        public virtual void SetConnectionStringName(string connectionStringName)
        {
            this.connectionStringName = connectionStringName;
        }

        #endregion

        #region 連線資訊 CreateConnectionInstance

        /// <summary>
        /// 建立連線資訊
        /// </summary>
        public SqlConnection CreateConnectionInstance()
        {
            //使用呼叫元件端的連線字串設定
            return new SqlConnection(ConfigurationManager.ConnectionStrings[GetConnectionStringName()].ConnectionString);
        }

        /// <summary>
        /// 建立連線資訊並開啟連線
        /// </summary>
        public SqlConnection CreateConnectionInstanceWithOpen()
        {
            SqlConnection conn = null;

            //建立連線資訊
            conn = CreateConnectionInstance();
            //開啟連線
            conn.Open();

            //停止在結果中傳回顯示 Transact-SQL 陳述式所影響之資料列數的訊息
            SqlCommand cmdSetNoCount = new SqlCommand("set nocount on", conn);
            cmdSetNoCount.ExecuteNonQuery();

            return conn;
        }

        /// <summary>
        /// 關閉連線資訊
        /// </summary>
        public void CloseConnection(IDbConnection conn)
        {
            if (conn == null)
                return;

            conn.Close();
        }

        /// <summary>
        /// 關閉DataReader
        /// </summary>
        public void CloseDataReader(IDataReader rdr, IDbConnection conn)
        {
            if (rdr != null)
                rdr.Close();

            CloseConnection(conn);
        }

        #endregion
    }
}
