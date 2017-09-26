using Common.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// 所有資料存取來源
/// </summary>
public class DBs
{
    /// <summary>
    /// 主要系統資料存取來源
    /// </summary>
    public static IDataAccessSource MainDB
    {
        get
        {
            return new DataAccessSource("DBConnString");
        }
    }

    private DBs()
    {
    }
}