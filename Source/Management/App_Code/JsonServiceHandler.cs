using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Common.LogicObject;
using System.Web.SessionState;

public abstract class JsonServiceHandlerAbstract : IJsonServiceHandler
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

    public JsonServiceHandlerAbstract(HttpContext context)
    {
        this.context = context;
    }

    protected string GetParamValue(string name)
    {
        if (allowedParamFromQueryString)
            return GetSafeStringExtensions.ToSafeStr(Request[name]);

        return GetSafeStringExtensions.ToSafeStr(Request.Form[name]);
    }

    public abstract ClientResult ProcessRequest();
}

/// <summary>
/// 暫存身分的權限
/// </summary>
public class TemporarilyStoreRolePrivilege : JsonServiceHandlerAbstract
{
    public TemporarilyStoreRolePrivilege(HttpContext context)
        : base(context)
    {
    }

    public override ClientResult ProcessRequest()
    {
        throw new NotImplementedException();
    }
}