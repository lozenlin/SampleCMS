<%@ WebHandler Language="C#" Class="ToSearchResult" %>

using System;
using System.Web;
using Common.LogicObject;

public class ToSearchResult : IHttpHandler
{
    protected SearchPageCommon c;
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

    #endregion

    public void ProcessRequest(HttpContext context)
    {
        c = new SearchPageCommon(context, null);
        c.InitialLoggerOfUI(this.GetType());

        this.context = context;

        string keyWord = c.qsQueryKeyword.Trim();
        c.GoToSearchResult(keyWord);
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}