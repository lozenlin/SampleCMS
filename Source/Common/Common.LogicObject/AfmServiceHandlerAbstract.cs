using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;

namespace Common.LogicObject
{
    /// <summary>
    /// angular-FileManager service handler abstract
    /// </summary>
    public abstract class AfmServiceHandlerAbstract : IAfmServiceHandler
    {
        protected HttpContext context = null;
        protected bool allowedParamFromQueryString = false;   // true for testing

        #region 工具屬性

        protected HttpServerUtility Server
        {
            get { return context.Server; }
        }

        protected HttpRequest Request
        {
            get { return context.Request; }
        }

        protected HttpResponse Response
        {
            get { return context.Response; }
        }

        protected HttpSessionState Session
        {
            get { return context.Session; }
        }

        #endregion

        public AfmServiceHandlerAbstract(HttpContext context)
        {
            this.context = context;
        }

        protected string GetParamValue(string name)
        {
            if (allowedParamFromQueryString)
                return GetSafeStringExtensions.ToSafeStr(Request[name]);

            return GetSafeStringExtensions.ToSafeStr(Request.Form[name]);
        }

        public abstract AfmResult ProcessRequest();
    }
}
