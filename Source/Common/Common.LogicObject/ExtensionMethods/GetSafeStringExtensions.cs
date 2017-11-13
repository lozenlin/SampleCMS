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

        public static T To<T>(this DataRowView drv, string property, T nullValue)
        {
            T result = default(T);

            object obj = drv[property];

            if (obj == null || Convert.IsDBNull(obj))
            {
                result = nullValue;
            }
            else
            {
                result = (T)obj;
            }

            if (obj is string && obj != null)
            {
                //result = (T)Convert.ChangeType(AntiXss.GetSafeHtmlFragment(obj.ToString()), typeof(T));
            }

            return result;
        }
    }
}
