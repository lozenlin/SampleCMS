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
    public static IAfmServiceHandler GetHandler(HttpContext context, string action)
    {
        IAfmServiceHandler handler = null;

        switch (action)
        {
            case "list":
                handler = new AfmGetList(context);
                break;
        }

        return handler;
    }
}