using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Common.LogicObject
{
    /// <summary>
    /// angular-FileManager service page common function
    /// </summary>
    public class AfmServicePageCommon : PageCommon
    {
        public AfmServicePageCommon(HttpContext context)
            : base(context, null)
        {
        }

        #region qs:=QueryString, se:=Session, vs:=ViewState, co:=Cookie

        public string qsListType
        {
            get
            {
                return QueryStringToSafeStr("listtype") ?? "";
            }
        }

        #endregion
    }
}
