using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.LogicObject
{
    public class BasePage : System.Web.UI.Page
    {
        /// <summary>
        /// 讓 EvalToSafeStr() 可依照前台、後台去調整過濾方式 
        /// </summary>
        protected bool isBackendPage = false;

        public BasePage()
            : base()
        {
        }

        public string EvalToSafeStr(string expression)
        {
            return EvalToSafeStr(expression, null);
        }

        public string EvalToSafeStr(string expression, string format)
        {
            string result = "";

            if (format == null)
                result = Convert.ToString(Eval(expression));
            else
                result = Eval(expression, format);

            //if (!isBackendPage)
            //result = AntiXss.GetSafeHtmlFragment(result).Replace("&amp;", "&");

            return result;
        }
    }
}
