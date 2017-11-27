using Common.LogicObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
                handler = new AfmRemoveDirOrFiles(context, afmRequest);
                break;
            case "createFolder":
                handler = new AfmCreateFolder(context, afmRequest);
                break;
        }

        return handler;
    }
}