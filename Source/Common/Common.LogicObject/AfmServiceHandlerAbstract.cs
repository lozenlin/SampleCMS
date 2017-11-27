using System;
using System.Collections.Generic;
using System.IO;
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
        protected EmployeeAuthorityLogic empAuth = null;

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
            empAuth = new EmployeeAuthorityLogic(c);
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

        protected string GetListTypeSubDir()
        {
            string listTypeDir = "";

            if (string.Compare(c.qsListType, AfmListType.icon) == 0)
            {
                listTypeDir = @"BPimages\icon\";
            }
            else if (string.Compare(c.qsListType, AfmListType.images) == 0)
            {
                listTypeDir = @"images\";
            }
            else if (string.Compare(c.qsListType, AfmListType.UserFiles) == 0)
            {
                listTypeDir = @"UserFiles\";
            }

            return listTypeDir;
        }

        protected string GetListDir()
        {
            return GetListDir(afmRequest.path, AfmFileType.dir);
        }

        protected string GetListDir(string path, string afmFileType)
        {
            string listDir = "";

            string appDir = Server.MapPath("~/");
            string listTypeSubDir = GetListTypeSubDir();

            if (listTypeSubDir != "")
            {
                listDir = appDir + listTypeSubDir;

                if (!string.IsNullOrEmpty(path))
                {
                    string subPath = path.Substring(1).Replace('/', '\\');
                    listDir += subPath;

                    if (afmFileType == AfmFileType.dir)
                    {
                        listDir += @"\";
                    }
                }
            }

            return listDir;
        }

        protected bool IsFileExists(string item)
        {
            string fileFullName = GetListDir(item, AfmFileType.file);
            return File.Exists(fileFullName);
        }

        protected bool IsDirectoryExists(string item)
        {
            string dirFullName = GetListDir(item, AfmFileType.dir);
            return Directory.Exists(dirFullName);
        }
    }
}
