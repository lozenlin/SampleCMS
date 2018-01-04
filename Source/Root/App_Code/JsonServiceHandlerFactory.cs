using Common.LogicObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public static class JsonServiceHandlerFactory
{
    public static IJsonServiceHandler GetHandler(HttpContext context, string serviceName)
    {
        IJsonServiceHandler handler = null;

        switch (serviceName)
        {
            case "Article_GetListWithThumb":
                handler = new Article_GetListWithThumb(context);
                break;
        }

        return handler;
    }
}
