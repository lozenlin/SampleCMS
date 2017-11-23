using Common.LogicObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// get list for angular-FileManager
/// </summary>
public class AfmGetList : AfmServiceHandlerAbstract
{
    public AfmGetList(HttpContext context)
        : base(context)
    {
    }

    public override AfmResult ProcessRequest()
    {
        AfmResultOfResult ror = new AfmResultOfResult()
        {
            success = false,
            error = "not implemented"
        };

        AfmResult result = new AfmResult()
        {
            result = ror
        };

        return result;
    }
}