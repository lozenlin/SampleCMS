<%@ WebHandler Language="C#" Class="afmDownload" %>

using System;
using System.Web;
using System.Web.SessionState;
using Common.LogicObject;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Web.Caching;

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

    protected Cache Cache
    {
        get { return context.Cache; }
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
        ImageFormat contentImageFormat = ImageFormat.Jpeg;

        switch (fi.Extension.ToLower())
        {
            case ".jpg":
            case ".jpeg":
                contentType = "image/jpeg";
                contentImageFormat = ImageFormat.Jpeg;
                break;
            case ".bmp":
                contentType = "image/bmp";
                contentImageFormat = ImageFormat.Bmp;
                break;
            case ".gif":
                contentType = "image/gif";
                contentImageFormat = ImageFormat.Gif;
                break;
            case ".png":
                contentType = "image/png";
                contentImageFormat = ImageFormat.Png;
                break;
            case ".tiff":
            case ".tif":
                contentType = "image/tiff";
                contentImageFormat = ImageFormat.Tiff;
                break;
        }

        byte[] bytes = null;

        if (c.qsThumb == 1 && contentType.StartsWith("image"))
        {
            //縮圖

            //檢查快取
            bool useCacheFunction = true;
            string cacheKey = string.Format("{0}:lt{1}:pa{2}:th{3}",
                Request.AppRelativeCurrentExecutionFilePath, c.qsListType, c.qsPath, 
                c.qsThumb);
            Size picFitSize = new Size(120, 120);
            Size destSize = picFitSize;
            object cachedImageBytes = null;

            if (useCacheFunction)
            {
                cachedImageBytes = Cache.Get(cacheKey);
            }

            if (cachedImageBytes == null)
            {
                System.Drawing.Image img = System.Drawing.Image.FromFile(fileFullName);

                float widthRate = img.Width / (float)picFitSize.Width;
                float heightRate = img.Height / (float)picFitSize.Height;

                if (widthRate > heightRate)
                {
                    destSize.Width = Convert.ToInt32(img.Width / widthRate);
                    destSize.Height = Convert.ToInt32(img.Height / widthRate);
                }
                else
                {
                    destSize.Width = Convert.ToInt32(img.Width / heightRate);
                    destSize.Height = Convert.ToInt32(img.Height / heightRate);
                }

                System.Drawing.Image imgFit = img.GetThumbnailImage(destSize.Width, destSize.Height, new System.Drawing.Image.GetThumbnailImageAbort(() => false), IntPtr.Zero);

                //還原旋轉方向
                // reference: https://forums.asp.net/t/2016582.aspx?Resize+script+is+rotating+image+sometimes+
                if (new List<int>(img.PropertyIdList).Contains(0x112))
                {
                    PropertyItem prop = img.GetPropertyItem(0x112);

                    if (prop.Type == 3 && prop.Len == 2)
                    {
                        UInt16 orientationExif = BitConverter.ToUInt16(prop.Value, 0);

                        switch (orientationExif)
                        {
                            case 8:
                                imgFit.RotateFlip(RotateFlipType.Rotate270FlipNone);
                                break;
                            case 3:
                                imgFit.RotateFlip(RotateFlipType.Rotate180FlipNone);
                                break;
                            case 6:
                                imgFit.RotateFlip(RotateFlipType.Rotate90FlipNone);
                                break;
                        }
                    }
                }

                MemoryStream ms = new MemoryStream();
                imgFit.Save(ms, contentImageFormat);
                imgFit.Dispose();
                img.Dispose();

                ms.Position = 0;
                bytes = new byte[ms.Length];
                ms.Read(bytes, 0, bytes.Length);
                ms.Close();
                
                //加入快取
                if (useCacheFunction)
                {
                    Cache.Insert(cacheKey, bytes, null,
                        DateTime.Now.AddMinutes(1), Cache.NoSlidingExpiration, CacheItemPriority.Normal,
                        null);
                }
            }
            else
            {
                bytes = (byte[])cachedImageBytes;
            }
        }

        //使用 Client Cache
        Response.Cache.SetCacheability(HttpCacheability.Private);    // Private:Client, Public:Server+Proxy+Client, Server:Client No-Cache
        Response.Cache.VaryByParams["listtype"] = true;
        Response.Cache.VaryByParams["path"] = true;
        Response.Cache.VaryByParams["thumb"] = true;
        Response.Cache.SetExpires(DateTime.Now.AddYears(1));    // for Client
        
        Response.ContentType = contentType;
        Response.HeaderEncoding = System.Text.Encoding.GetEncoding("big5");
        Response.AddHeader("Content-Disposition", "attachment; filename=" + fi.Name);

        if (bytes != null && bytes.Length > 0)
        {
            Response.BinaryWrite(bytes);
        }
        else
        {
            Response.WriteFile(fileFullName);
        }
        
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