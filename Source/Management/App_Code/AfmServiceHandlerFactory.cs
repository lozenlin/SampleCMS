using Common.LogicObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AfmService
{
    /// <summary>
    /// angular-FileManager service handler factory
    /// </summary>
    public static class AfmServiceHandlerFactory
    {
        public static IAfmServiceHandler GetHandler(HttpContext context, AfmRequest afmRequest)
        {
            IAfmServiceHandler handler = null;

            if (afmRequest == null)
                return handler;

            switch (afmRequest.action)
            {
                case "list":
                    handler = new AfmGetList(context, afmRequest);
                    break;
                case "upload":
                    handler = new AfmUploadFiles(context, afmRequest);
                    break;
                case "remove":
                    handler = new AfmRemoveFoldersOrFiles(context, afmRequest);
                    break;
                case "createFolder":
                    handler = new AfmCreateFolder(context, afmRequest);
                    break;
                case "rename":
                    handler = new AfmRenameFolderOrFile(context, afmRequest);
                    break;
            }

            return handler;
        }
    }
}