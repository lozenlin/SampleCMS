<%@ WebHandler Language="C#" Class="captch" %>

using System;
using System.Web;
using System.Web.SessionState;
using Common.LogicObject;

/// <summary>
/// 
/// </summary>
/// <remarks>
/// reference: https://www.codeproject.com/Articles/169371/Captcha-Image-using-C-in-ASP-NET
/// </remarks>
public class captch : IHttpHandler, IRequiresSessionState
{
    public void ProcessRequest(HttpContext context)
    {
        BackendPageCommon c = new BackendPageCommon(context, null);

        string captchaCode = Common.Utility.StringUtility.GenerateCaptchaCode(5);
        // save into session
        c.seCaptchaCode = captchaCode;

        RandomImage rndImg = new RandomImage(captchaCode, 320, 70);

        context.Response.ContentType = "image/jpeg";
        context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
        rndImg.Image.Save(context.Response.OutputStream, System.Drawing.Imaging.ImageFormat.Jpeg);
        rndImg.Dispose();
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}