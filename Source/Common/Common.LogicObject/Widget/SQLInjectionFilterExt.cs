using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.LogicObject
{
    /// <summary>
    /// SQL Injection 過濾加強版,可測試運算式
    /// </summary>
    public class SQLInjectionFilterExt : SQLInjectionFilter
    {
        /// <summary>
        /// SQL Injection 過濾加強版,可測試運算式
        /// </summary>
        public SQLInjectionFilterExt()
            : base()
        {
        }

        /// <summary>
        /// 測試運算式是否成立,能否被用來做SQL Injection
        /// </summary>
        protected override bool IsSQLInjectionExpr(string expr)
        {
            SQLInjectionFilterLogic sqlInjectionFilterLogic = new SQLInjectionFilterLogic();

            return sqlInjectionFilterLogic.IsSQLInjectionExpr(expr);
        }
    }
}
