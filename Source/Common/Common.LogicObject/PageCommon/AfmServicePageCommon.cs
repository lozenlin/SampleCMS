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
    public class AfmServicePageCommon : BackendPageCommon
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

        public string qsPath
        {
            get
            {
                return QueryStringToSafeStr("path") ?? "";
            }
        }

        /// <summary>
        /// Thumbnail picture mode (1:active, 0:nothing)
        /// </summary>
        public int qsThumb
        {
            get
            {
                int result = 0;
                string str = QueryStringToSafeStr("thumb");

                if (str != null && int.TryParse(str, out result))
                {
                    if (result != 1)
                        result = 0;
                }

                return result;
            }
        }

        public string seLastPath
        {
            get { return SessionToSafeStr("seLastPath"); }
            set { Session["seLastPath"] = value; }
        }

        #endregion
    }
}
