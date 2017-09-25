using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data.SqlClient;

namespace Common.DataAccess
{
    /// <summary>
    /// 資料層用的參數資訊
    /// </summary>
    public class SqlParaInfo
    {
        public string Name;
        public object Value;
        public bool IsOutput;
        public Type ParaType;
        public FieldInfo ParaFieldInfo;
        public SqlParameter SqlPara;
    }
}
