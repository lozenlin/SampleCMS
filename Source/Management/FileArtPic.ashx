<%@ WebHandler Language="C#" Class="FileArtPic" %>

using System;
using System.Web;
using Common.LogicObject;

/// <summary>
/// 網頁照片下載
/// </summary>
public class FileArtPic : IHttpHandler
{
    protected ArtPicDownloadCommon c;

    public void ProcessRequest(HttpContext context)
    {
        c = new ArtPicDownloadCommon(context, null);

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