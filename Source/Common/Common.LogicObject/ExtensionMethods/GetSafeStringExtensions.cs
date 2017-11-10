using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Common.LogicObject
{
    public static class GetSafeStringExtensions
    {
        public static string ToSafeStr(this DataRow dr, string columnName)
        {
            object obj = dr[columnName];

            if (obj == null)
                return null;

            //return AntiXss.GetSafeHtmlFragment(obj.ToString());
            return obj.ToString();
        }

        public static string ToSafeStr(this DataRowView drv, string property)
        {
            object obj = drv[property];

            if (obj == null)
                return null;

            //return AntiXss.GetSafeHtmlFragment(obj.ToString());
            return obj.ToString();
        }
    }
}
