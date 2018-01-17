using Common.DataAccess;
using Common.DataAccess.SQLInjectionFilter;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.LogicObject
{
    public class SQLInjectionFilterLogic
    {
        protected ILog logger = null;
        protected string dbErrMsg = "";

        public SQLInjectionFilterLogic()
        {
            logger = LogManager.GetLogger(this.GetType());
        }

        // DataAccess functions

        /// <summary>
        /// DB command 執行後的錯誤訊息
        /// </summary>
        public string GetDbErrMsg()
        {
            return dbErrMsg;
        }

        /// <summary>
        /// 測試運算式是否成立,能否被用來做 SQL Injection
        /// </summary>
        public bool IsSQLInjectionExpr(string expr)
        {
            IDataAccessCommand cmd = DataAccessCommandFactory.GetDataAccessCommand(DBs.MainDB);
            spIsSQLInjectionExpr cmdInfo = new spIsSQLInjectionExpr() { Expr = expr };

            bool errCode = false;
            bool result = cmd.ExecuteScalar<bool>(cmdInfo, errCode);
            dbErrMsg = cmd.GetErrMsg();

            return result;
        }
    }
}
