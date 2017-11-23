<%@ WebHandler Language="C#" Class="afmService" %>

using System;
using System.Web;
using Common.LogicObject;
using System.Web.SessionState;
using Newtonsoft.Json;
using System.IO;

/// <summary>
/// angular-FileManager Service
/// </summary>
public class afmService : IHttpHandler
{
    protected PageCommon c;
    protected HttpContext context;

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

    private bool allowedParamFromQueryString = false;   // true for testing

    public void ProcessRequest(HttpContext context)
    {
        c = new PageCommon(context, null);
        c.InitialLoggerOfUI(this.GetType());

        this.context = context;
        AfmResult afmResult = null;

        StreamReader sr = new StreamReader(Request.InputStream);
        string payload = sr.ReadToEnd();
        sr.Close();

        try
        {
            dynamic reqData = JsonConvert.DeserializeObject(payload);

            if (reqData == null)
                throw new Exception("payload is invalid");
            
            string action = reqData.action;

            IAfmServiceHandler handler = AfmServiceHandlerFactory.GetHandler(context, action);

            if (handler == null)
                throw new Exception("action is invalid");

            afmResult = handler.ProcessRequest();
        }
        catch (Exception ex)
        {
            c.LoggerOfUI.Error("", ex);

            AfmResultOfResult ror = new AfmResultOfResult()
            {
                success = false,
                error = ex.Message
            };

            afmResult = new AfmResult()
            {
                result = ror
            };
        }

        string result = JsonConvert.SerializeObject(afmResult);

        Response.ContentType = "text/plain";
        Response.Write(result);
    }

    private string GetParamValue(string name)
    {
        if (allowedParamFromQueryString)
            return GetSafeStringExtensions.ToSafeStr(Request[name]);

        return c.FormToSafeStr(name);
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}