using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Common.LogicObject
{
    public static class GetSafeStringExtensions
    {
        /// <summary>
        /// 讓 ToSafeStr() 可依照前台、後台去調整過濾方式
        /// </summary>
        public static bool IsBackendPage
        {
            get { return isBackendPage; }
            set { isBackendPage = value; }
        }
        private static bool isBackendPage = false;

        public static string ToSafeStr(this DataRow dr, string columnName)
        {
            object obj = dr[columnName];

            if (obj == null)
                return null;

            //if (!isBackendPage)
            //return AntiXss.GetSafeHtmlFragment(obj.ToString());
            return obj.ToString();
        }

        public static string ToSafeStr(this DataRowView drv, string property)
        {
            object obj = drv[property];

            if (obj == null)
                return null;

            //if (!isBackendPage)
            //return AntiXss.GetSafeHtmlFragment(obj.ToString());
            return obj.ToString();
        }

        public static string ToSafeStr(string value)
        {
            //if (!isBackendPage)
            //return AntiXss.GetSafeHtmlFragment(value);
            return value;
        }

        public static T To<T>(this DataRow dr, string columnName, T nullValue)
        {
            T result = default(T);

            object obj = dr[columnName];

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
                //if (!isBackendPage)
                //result = (T)Convert.ChangeType(AntiXss.GetSafeHtmlFragment(obj.ToString()), typeof(T));
            }

            return result;
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
                //if (!isBackendPage)
                //result = (T)Convert.ChangeType(AntiXss.GetSafeHtmlFragment(obj.ToString()), typeof(T));
            }

            return result;
        }
    }
}
