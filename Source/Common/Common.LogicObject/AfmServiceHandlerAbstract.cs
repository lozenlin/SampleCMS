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
        protected AfmRequest afmRequest = null;
        protected AfmServicePageCommon c = null;

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

        public AfmServiceHandlerAbstract(HttpContext context, AfmRequest afmRequest)
        {
            this.context = context;
            this.afmRequest = afmRequest;
            c = new AfmServicePageCommon(context);
            c.InitialLoggerOfUI(this.GetType());
        }

        public AfmResult BuildResultOfError(string errMsg)
        {
            AfmResultOfResult ror = new AfmResultOfResult()
            {
                success = false,
                error = errMsg
            };

            AfmResult result = new AfmResult()
            {
                result = ror
            };

            return result;
        }

        public AfmResult BuildResultOfSuccess()
        {
            AfmResultOfResult ror = new AfmResultOfResult()
            {
                success = true
            };

            AfmResult result = new AfmResult()
            {
                result = ror
            };

            return result;
        }

        public abstract AfmResult ProcessRequest();

        protected string GetListDir()
        {
            string listDir = "";

            string appDir = Server.MapPath("~/");

            if (string.Compare(c.qsListType, AfmListType.icon) == 0)
            {
                listDir = appDir + @"BPimages\icon\";
            }
            else if (string.Compare(c.qsListType, AfmListType.images) == 0)
            {
                listDir = appDir + @"images\";
            }
            else if (string.Compare(c.qsListType, AfmListType.UserFiles) == 0)
            {
                listDir = appDir + @"UserFiles\";
            }

            if (afmRequest.path.Length > 1)
            {
                string subPath = afmRequest.path.Substring(1).Replace('/', '\\');
                listDir += subPath + @"\";
            }

            return listDir;
        }
    }
}
