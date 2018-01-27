using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;

namespace Common.LogicObject
{
    public abstract class JsonServiceHandlerAbstract : IJsonServiceHandler
    {
        protected HttpContext context = null;
        protected ILog logger;
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

        public JsonServiceHandlerAbstract(HttpContext context)
        {
            this.context = context;
            logger = LogManager.GetLogger(this.GetType());
        }

        protected string GetParamValue(string name)
        {
            if (allowedParamFromQueryString)
                return GetSafeStringExtensions.ToSafeStr(Request[name]);

            return GetSafeStringExtensions.ToSafeStr(Request.Form[name]);
        }

        public abstract ClientResult ProcessRequest();
    }
}
