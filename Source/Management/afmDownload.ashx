<%@ WebHandler Language="C#" Class="afmDownload" %>

using System;
using System.Web;
using System.Web.SessionState;
using Common.LogicObject;
using System.IO;

public class afmDownload : IHttpHandler, IRequiresSessionState
{
    protected AfmServicePageCommon c;
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

    public void ProcessRequest(HttpContext context)
    {
        c = new AfmServicePageCommon(context);
        c.InitialLoggerOfUI(this.GetType());

        this.context = context;

        string fileFullName = GetFileFullName();

        if (fileFullName == "")
        {
            return;
        }

        FileInfo fi = new FileInfo(fileFullName);

        if (!fi.Exists)
        {
            return;
        }

        //設定ContentType
        string contentType = "application/octet-stream";

        switch (fi.Extension.ToLower())
        {
            case ".jpg":
            case ".jpeg":
                contentType = "image/jpeg";
                break;
            case ".bmp":
                contentType = "image/bmp";
                break;
            case ".gif":
                contentType = "image/gif";
                break;
            case ".png":
                contentType = "image/png";
                break;
            case ".tiff":
            case ".tif":
                contentType = "image/tiff";
                break;
        }

        //使用 Client Cache
        Response.Cache.SetCacheability(HttpCacheability.Private);    // Private:Client, Public:Server+Proxy+Client, Server:Client No-Cache
        Response.Cache.VaryByParams["listtype"] = true;
        Response.Cache.VaryByParams["path"] = true;
        Response.Cache.SetExpires(DateTime.Now.AddYears(1));    // for Client
        
        Response.ContentType = contentType;
        Response.HeaderEncoding = System.Text.Encoding.GetEncoding("big5");
        Response.AddHeader("Content-Disposition", "attachment; filename=" + fi.Name);
        Response.WriteFile(fileFullName);
        Response.End();
    }

    private string GetFileFullName()
    {
        string fileFullName = "";

        string appDir = Server.MapPath("~/");

        if (string.Compare(c.qsListType, AfmListType.icon) == 0)
        {
            fileFullName = appDir + @"BPimages\icon\";
        }
        else if (string.Compare(c.qsListType, AfmListType.images) == 0)
        {
            fileFullName = appDir + @"images\";
        }
        else if (string.Compare(c.qsListType, AfmListType.UserFiles) == 0)
        {
            fileFullName = appDir + @"UserFiles\";
        }

        if (fileFullName != "")
        {
            if (!string.IsNullOrEmpty(c.qsPath))
            {
                string subPath = c.qsPath.Substring(1).Replace('/', '\\');
                fileFullName += subPath;
            }
        }

        return fileFullName;
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}