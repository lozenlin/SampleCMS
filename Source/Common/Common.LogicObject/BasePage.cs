using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.LogicObject
{
    public class BasePage : System.Web.UI.Page
    {
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

            //result = AntiXss.GetSafeHtmlFragment(result).Replace("&amp;", "&");

            return result;
        }
    }
}
