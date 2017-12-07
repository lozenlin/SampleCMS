<%@ WebHandler Language="C#" Class="FileAtt" %>

using System;
using System.Web;
using Common.LogicObject;

/// <summary>
/// 附件檔案下載
/// </summary>
public class FileAtt : IHttpHandler
{
    protected AttDownloadCommon c;

    public void ProcessRequest(HttpContext context)
    {
        c = new AttDownloadCommon(context, null);

        c.IsInBackend = true;

        if (!c.ProcessRequest())
        {
            context.Response.Redirect("ErrorPage.aspx", true);
        }
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}