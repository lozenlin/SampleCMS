<%@ WebHandler Language="C#" Class="GetCurrentAccount" %>

using System;
using System.Web;
using System.Web.SessionState;
using Common.LogicObject;
using Newtonsoft.Json;
using Common.Utility;
using System.Configuration;

public class GetCurrentAccount : IHttpHandler, IRequiresSessionState
{
    protected SsoAuthenticatorCommon c;
    protected HttpContext context;

    private string aesKeyOfFP = "";   // 16 letters
    private string aesKeyOfBP = "";   // 16 letters
    private string basicIV = "";  // 16 letters
    private string[] locationSections = null;
    private string domainName = "";
    private string pageName = "";

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

    public GetCurrentAccount()
    {
        aesKeyOfFP = ConfigurationManager.AppSettings["AesKeyOfFP"];
        aesKeyOfBP = ConfigurationManager.AppSettings["AesKeyOfBP"];
        basicIV = ConfigurationManager.AppSettings["AesIV"];
    }

    public void ProcessRequest(HttpContext context)
    {
        c = new SsoAuthenticatorCommon(context, null);
        c.InitialLoggerOfUI(this.GetType());

        this.context = context;

        if (!IsParamValid())
        {
            Response.Write("Invalid Parameters!");
            return;
        }

        try
        {
            if (c.qsPreview == "1")
            {
                AuthenticateToPreview();
            }
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
            return;
        }
    }

    private bool IsParamValid()
    {
        bool isParamValid = true;

        if (string.IsNullOrEmpty(c.qsToken) || string.IsNullOrEmpty(c.qsLocation))
        {
            isParamValid = false;
        }
        else
        {
            locationSections = c.qsLocation.Split('/');

            if (locationSections.Length > 2)
            {
                domainName = locationSections[2];
            }

            if (locationSections.Length > 3)
            {
                pageName = locationSections[locationSections.Length - 1];
            }

            if (string.IsNullOrEmpty(domainName) || string.IsNullOrEmpty(pageName))
            {
                isParamValid = false;
            }
        }

        return isParamValid;
    }

    private void AuthenticateToPreview()
    {
        try
        {
            string valueOfToken = AesUtility.Decrypt(c.qsToken, aesKeyOfFP, basicIV);
            DateTime validTimeOfFP = Convert.ToDateTime(valueOfToken);

            if ((DateTime.Now - validTimeOfFP).TotalMinutes > 5)
            {
                throw new Exception("exceed valid time of FrontendPage");
            }
        }
        catch (Exception ex)
        {
            c.LoggerOfUI.Error("", ex);
            throw new Exception("Invalid token");
        }

        PreviewArticle previewArticle = new PreviewArticle();

        if (c.IsAuthenticated())
        {
            previewArticle.EmpAccount = c.seLoginEmpData.EmpAccount;
            previewArticle.ArticleId = c.qsArtId.ToString();

            if (string.Compare(pageName, "Article.aspx", true) == 0
                && c.qsArtId == Guid.Empty)
            {
                pageName = "Index.aspx";
            }
        }

        previewArticle.ValidTime = DateTime.Now.AddMinutes(10);
        string previewToken = "";

        try
        {
            string previewValueInToken = JsonConvert.SerializeObject(previewArticle);
            previewToken = AesUtility.Encrypt(previewValueInToken, aesKeyOfBP, basicIV);
        }
        catch (Exception ex)
        {
            c.LoggerOfUI.Error("", ex);
            throw new Exception("Generate result failed.");
        }
        
        string websiteUrl = ConfigurationManager.AppSettings["WebsiteUrl"];
        string url = websiteUrl + "/" + pageName + "?preview=" + Server.UrlEncode(previewToken) + "&l=" + c.qsLangNo;
        Response.Redirect(url);
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}